using MediatR;
using Ticketing.Application.Services.Interfaces;

namespace Ticketing.Application.Commands.AddTicketReply;
public class AddTicketReplyCommandHandler : IRequestHandler<AddTicketReplyCommand, Guid>
{
  private readonly ITicketService _ticketService;

  public AddTicketReplyCommandHandler(ITicketService ticketService)
  {
    _ticketService = ticketService;
  }

  public async Task<Guid> Handle(AddTicketReplyCommand request, CancellationToken cancellationToken)
  {
    return await _ticketService.AddReplyAsync(request.TicketId, request.Text, request.UserId, cancellationToken);
  }
}
