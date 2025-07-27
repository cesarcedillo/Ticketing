using MediatR;
using Ticketing.BFF.Application.Services.Interfaces;

namespace Ticketing.BFF.Application.Commands.Ticket.AddTicketReply;
public class AddTicketReplyCommandBffHandler : IRequestHandler<AddTicketReplyCommandBff>
{
  private readonly ITicketService _ticketService;

  public AddTicketReplyCommandBffHandler(ITicketService ticketService)
  {
    _ticketService = ticketService;
  }

  public async Task Handle(AddTicketReplyCommandBff request, CancellationToken cancellationToken)
  {
    await _ticketService.AddReplyAsync(request.TicketId, request.Text, request.UserId, cancellationToken);
  }
}
