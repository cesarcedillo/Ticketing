using MediatR;
using Ticketing.Ticket.Application.Dtos.Responses;
using Ticketing.Ticket.Application.Services.Interfaces;

namespace Ticketing.Ticket.Application.Queries.ListTickets;
public class ListTicketsQueryHandler : IRequestHandler<ListTicketsQuery, IReadOnlyList<TicketResponse>>
{
  private readonly ITicketService _ticketService;

  public ListTicketsQueryHandler(ITicketService ticketService)
  {
    _ticketService = ticketService;
  }

  public async Task<IReadOnlyList<TicketResponse>> Handle(ListTicketsQuery request, CancellationToken cancellationToken)
  {
    var listTicketsResponse = await _ticketService.ListTicketsAsync(request.Status, request.UserId, cancellationToken);

    return listTicketsResponse;
  }
}
