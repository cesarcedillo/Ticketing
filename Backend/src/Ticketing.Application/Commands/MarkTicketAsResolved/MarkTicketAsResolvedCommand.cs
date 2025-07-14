using MediatR;

namespace Ticketing.Application.Commands.MarkTicketAsResolved;

public sealed record MarkTicketAsResolvedCommand(Guid TicketId)
    : IRequest;