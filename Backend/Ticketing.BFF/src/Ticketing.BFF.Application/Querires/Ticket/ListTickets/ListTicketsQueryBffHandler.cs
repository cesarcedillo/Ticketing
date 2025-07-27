using MediatR;
using Ticketing.BFF.Application.Dto.Responses;
using Ticketing.BFF.Application.Services.Interfaces;

namespace Ticketing.BFF.Application.Querires.Ticket.ListTickets;
public class ListTicketsQueryBffHandler : IRequestHandler<ListTicketsQueryBff, IReadOnlyList<TicketResponseBff>>
{
  private readonly ITicketService _ticketService;

  public ListTicketsQueryBffHandler(ITicketService ticketService)
  {
    _ticketService = ticketService;
  }

  public async Task<IReadOnlyList<TicketResponseBff>> Handle(ListTicketsQueryBff request, CancellationToken cancellationToken)
  {
    var listTicketsResponse = await _ticketService.ListTicketsAsync(request.Status, request.UserId, cancellationToken);

    return listTicketsResponse;
  }
}
