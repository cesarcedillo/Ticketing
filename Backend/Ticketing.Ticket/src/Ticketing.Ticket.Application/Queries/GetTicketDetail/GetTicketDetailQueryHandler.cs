using MediatR;
using Ticketing.Ticket.Application.Dtos.Responses;
using Ticketing.Ticket.Application.Services.Interfaces;

namespace Ticketing.Ticket.Application.Queries.GetTicketDetail;

public class GetTicketDetailQueryHandler : IRequestHandler<GetTicketDetailQuery, TicketDetailResponse>
{
  private readonly ITicketService _ticketService;

  public GetTicketDetailQueryHandler(ITicketService ticketService)
  {
    _ticketService = ticketService;
  }

  public async Task<TicketDetailResponse> Handle(GetTicketDetailQuery request, CancellationToken cancellationToken)
  {
    var ticket = await _ticketService.GetTicketDetailAsync(request.TicketId, cancellationToken);

    if (ticket == null)
      throw new KeyNotFoundException($"Ticket {request.TicketId} not found.");

    return ticket;
  }
}

