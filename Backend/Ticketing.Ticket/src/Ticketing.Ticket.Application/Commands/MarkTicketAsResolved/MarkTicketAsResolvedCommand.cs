using MediatR;

namespace Ticketing.Ticket.Application.Commands.MarkTicketAsResolved;

public sealed record MarkTicketAsResolvedCommand(Guid TicketId)
    : IRequest;