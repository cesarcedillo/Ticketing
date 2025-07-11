using MediatR;

namespace Ticketing.Application.Commands.MarkTicketAsResolved;
public sealed record MarkTicketAsResolvedCommand : IRequest
{
  public Guid TicketId { get; set; }
}
