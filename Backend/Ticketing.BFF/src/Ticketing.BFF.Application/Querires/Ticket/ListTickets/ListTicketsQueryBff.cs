using MediatR;
using Ticketing.BFF.Application.Dto.Responses;

namespace Ticketing.BFF.Application.Querires.Ticket.ListTickets;
public sealed record ListTicketsQueryBff(string? Status, Guid? UserId) : IRequest<IReadOnlyList<TicketResponseBff>>;