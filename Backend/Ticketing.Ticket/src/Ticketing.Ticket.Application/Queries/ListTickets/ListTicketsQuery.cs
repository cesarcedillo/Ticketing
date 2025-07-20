using MediatR;
using Ticketing.Ticket.Application.Dtos.Responses;

namespace Ticketing.Ticket.Application.Queries.ListTickets;

public sealed record ListTicketsQuery(string? Status, Guid? UserId)
    : IRequest<IReadOnlyList<TicketResponse>>;
