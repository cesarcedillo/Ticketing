using MediatR;
using Ticketing.BFF.Application.Services.Interfaces;

namespace Ticketing.BFF.Application.Commands.Ticket.MarkTicketAsResolved;
public class MarkTicketAsResolvedCommandBffHandler : IRequestHandler<MarkTicketAsResolvedCommandBff>
{
  private readonly ITicketService _ticketService;

  public MarkTicketAsResolvedCommandBffHandler(ITicketService ticketService)
  {
    _ticketService = ticketService;
  }

  public async Task Handle(MarkTicketAsResolvedCommandBff request, CancellationToken cancellationToken)
  {
    await _ticketService.MarkTicketAsResolvedAsync(request.TicketId, cancellationToken);
  }
}
