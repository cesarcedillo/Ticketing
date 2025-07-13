using MediatR;

namespace Ticketing.Application.Commands.AddTicketReply;

public class AddTicketReplyCommand : IRequest<Guid>
{
  public Guid TicketId { get; set; }
  public string Text { get; set; } = default!;
  public Guid UserId { get; set; }
}
