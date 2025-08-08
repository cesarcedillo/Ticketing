using MediatR;

namespace Ticketing.Notifications.Application.Commands.TicketResolved;
public sealed record TicketResolvedCommand(Guid TicketId, Guid UserId) : IRequest;
