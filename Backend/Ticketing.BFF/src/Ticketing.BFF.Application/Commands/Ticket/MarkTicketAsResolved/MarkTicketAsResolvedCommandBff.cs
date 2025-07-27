using MediatR;

namespace Ticketing.BFF.Application.Commands.Ticket.MarkTicketAsResolved;
public sealed record MarkTicketAsResolvedCommandBff(Guid TicketId) : IRequest;