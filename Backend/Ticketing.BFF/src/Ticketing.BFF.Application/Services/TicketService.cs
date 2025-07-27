using AutoMapper;
using Ticket.Cliente.NswagAutoGen.HttpClientFactoryImplementation;
using Ticketing.BFF.Application.Dto.Responses;
using Ticketing.BFF.Application.Services.Interfaces;

namespace Ticketing.BFF.Application.Services;
public class TicketService : ITicketService
{
  private readonly ITicketClient _ticketClient;
  private readonly IMapper _mapper;

  public TicketService(ITicketClient ticketClient, IMapper mapper)
  {
    _ticketClient = ticketClient;
    _mapper = mapper;
  }

  public async Task AddReplyAsync(Guid ticketId, string text, Guid userId, CancellationToken cancellationToken)
  {
    var ticketReply = new AddTicketReplyRequest
    {
      Text = text,
      UserId = userId
    };
    await _ticketClient.AddTicketReplyAsync(ticketId, ticketReply, cancellationToken);
  }

  public async Task<Guid> CreateTicketAsync(string subject, string description, Guid userId, CancellationToken cancellationToken)
  {
    var createTicket = new CreateTicketRequest()
    {
      Subject = subject,
      Description = description,
      UserId = userId
    };

    return await _ticketClient.CreateTicketAsync(createTicket, cancellationToken);
  }

  public async Task<TicketDetailResponseBff> GetTicketDetailAsync(Guid ticketId, CancellationToken cancellationToken)
  {
    var ticket = await _ticketClient.GetTicketDetailAsync(ticketId, cancellationToken);
    return _mapper.Map<TicketDetailResponseBff>(ticket);
  }

  public async Task<IReadOnlyList<TicketResponseBff>> ListTicketsAsync(string? status, Guid? userId, CancellationToken cancellationToken)
  {
    var tickets = await _ticketClient.ListTicketsAsync(status, userId, cancellationToken);
    return _mapper.Map<IReadOnlyList<TicketResponseBff>>(tickets);
  }

  public async Task MarkTicketAsResolvedAsync(Guid ticketId, CancellationToken cancellationToken)
  {
    await _ticketClient.MarkAsResolvedAsync(ticketId, cancellationToken);
  }
}
