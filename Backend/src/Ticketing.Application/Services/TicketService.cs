using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Ticketing.Application.Dtos.Responses;
using Ticketing.Application.Services.Interfaces;
using Ticketing.Domain.Aggregates;
using Ticketing.Domain.Interfaces.Repositories;

namespace Ticketing.Application.Services;

public class TicketService : ITicketService
{
  private readonly ITicketRepository _ticketRepository;
  private readonly IUserRepository _userRepository;
  private readonly IMapper _mapper;

  public TicketService(ITicketRepository ticketRepository, IUserRepository userRepository, IMapper mapper)
  {
    _ticketRepository = ticketRepository;
    _userRepository = userRepository;
    _mapper = mapper;
  }

  public async Task<Guid> CreateTicketAsync(string subject, string description, Guid userId, CancellationToken cancellationToken)
  {
    var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

    if (user == null)
      throw new InvalidOperationException("User not found.");

    var ticket = new Ticket(subject, description, user);

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

  public async Task<TicketDetailResponse?> GetTicketDetailAsync(Guid ticketId, CancellationToken cancellationToken)
  {
    var ticket = await _ticketRepository.Query()
        .Where(t => t.Id == ticketId)
        .ProjectTo<TicketDetailResponse>(_mapper.ConfigurationProvider)
        .FirstOrDefaultAsync(cancellationToken);

    return ticket;
  }

  public async Task MarkTicketAsResolvedAsync(Guid ticketId, CancellationToken cancellationToken)
  {
    var ticket = await _ticketRepository.Query()
        .FirstOrDefaultAsync(t => t.Id == ticketId, cancellationToken);

    if (ticket == null)
      throw new KeyNotFoundException($"Ticket {ticketId} not found.");

    ticket.MarkAsResolved();
    await _ticketRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
  }

  public async Task AddReplyAsync(Guid ticketId, string text, Guid userId, CancellationToken cancellationToken)
  {
    await _ticketRepository.AddReplyAsync(ticketId, text, userId, cancellationToken);
    await _ticketRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
  }

}

