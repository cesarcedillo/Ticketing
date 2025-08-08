using AutoMapper;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using Newtonsoft.Json;
using Ticketing.Core.Service.Messenger.Interfaces;
using Ticketing.Ticket.Application.Dtos.Responses;
using Ticketing.Ticket.Application.IntegrationEvents.Constants.Topics;
using Ticketing.Ticket.Application.IntegrationEvents.Dtos;
using Ticketing.Ticket.Application.Services.Interfaces;
using Ticketing.Ticket.Domain.Interfaces.Repositories;
using TicketType = Ticketing.Ticket.Domain.Aggregates.Ticket;

namespace Ticketing.Ticket.Application.Services;

public class TicketService : ITicketService
{
  private readonly ITicketRepository _ticketRepository;
  private readonly IMessengerSendService _messengerSendService;
  private readonly IMapper _mapper;

  public TicketService(ITicketRepository ticketRepository,
      IMessengerSendService messengerSendService, IMapper mapper)
  {
    _ticketRepository = ticketRepository;
    _messengerSendService = messengerSendService;
    _mapper = mapper;
  }

  public async Task<Guid> CreateTicketAsync(string subject, string description, Guid userId, CancellationToken cancellationToken)
  {
    var ticket = new TicketType(subject, description, userId);

    _ticketRepository.Add(ticket);

    await _ticketRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

    return ticket.Id;
  }

  public async Task<IReadOnlyList<TicketResponse>> ListTicketsAsync(string? status, Guid? userId, CancellationToken cancellationToken)
  {
    var ticketsQuery = _ticketRepository.Query();

    if (!string.IsNullOrWhiteSpace(status))
    {
      ticketsQuery = ticketsQuery.Where(t =>
          t.Status.ToString().Equals(status, StringComparison.OrdinalIgnoreCase));
    }
    if (userId.HasValue)
    {
      ticketsQuery = ticketsQuery.Where(t => t.UserId == userId.Value);
    }

    var tickets = await ticketsQuery
            .OrderByDescending(t => t.CreatedAt)
            .ProjectTo<TicketResponse>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);


    return tickets;
  }

  public async Task<TicketDetailResponse> GetTicketDetailAsync(Guid ticketId, CancellationToken cancellationToken)
  {
    var ticket = await _ticketRepository.Query()
        .Where(t => t.Id == ticketId)
        .ProjectTo<TicketDetailResponse>(_mapper.ConfigurationProvider)
        .FirstOrDefaultAsync(cancellationToken);

    if (ticket == null)
      throw new KeyNotFoundException($"Ticket {ticketId} not found.");

    return ticket;
  }

  public async Task MarkTicketAsResolvedAsync(Guid ticketId, CancellationToken cancellationToken)
  {
    await _ticketRepository.UnitOfWork.BeginTransactionAsync(cancellationToken);
    await _messengerSendService.BeginTransactionAsync(cancellationToken);
    try
    {
      var ticket = await _ticketRepository.Query().FirstOrDefaultAsync(t => t.Id == ticketId, cancellationToken);

      if (ticket == null)
        throw new KeyNotFoundException($"Ticket {ticketId} not found.");

      ticket.MarkAsResolved();
      await _ticketRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

      var integrationEvent = new TicketResolvedIntegrationEvent(ticket.Id, ticket.UserId);
      await _messengerSendService.SendMessageAsync(JsonConvert.SerializeObject(integrationEvent), 
                                                  IntegrationTopics.TicketResolved, 
                                                  cancellationToken);

      await _messengerSendService.CommitAsync(cancellationToken);
      await _ticketRepository.UnitOfWork.CommitAsync(cancellationToken);
    }
    catch
    {
      await _messengerSendService.RollBackAsync(cancellationToken);
      await _ticketRepository.UnitOfWork.RollbackAsync(cancellationToken);
      throw;
    }
  }

  public async Task AddReplyAsync(Guid ticketId, string text, Guid userId, CancellationToken cancellationToken)
  {
    await _ticketRepository.AddReplyAsync(ticketId, text, userId, cancellationToken);
    await _ticketRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
  }

}

