using MediatR;
using Ticketing.Application.Dtos.Responses;

namespace Ticketing.Application.Queries.ListTickets;

public sealed record ListTicketsQuery(string? Status, Guid? UserId)
    : IRequest<IReadOnlyList<TicketResponse>>;
