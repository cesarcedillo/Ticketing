using MediatR;
using Ticketing.Application.Services.Interfaces;

namespace Ticketing.Application.Commands.AddTicketReply;
public class AddTicketReplyCommandHandler : IRequestHandler<AddTicketReplyCommand>
{
  private readonly ITicketService _ticketService;

  public AddTicketReplyCommandHandler(ITicketService ticketService)
  {
    _ticketService = ticketService;
  }

  public async Task Handle(AddTicketReplyCommand request, CancellationToken cancellationToken)
  {
    await _ticketService.AddReplyAsync(request.TicketId, request.Text, request.UserId, cancellationToken);
  }
}
