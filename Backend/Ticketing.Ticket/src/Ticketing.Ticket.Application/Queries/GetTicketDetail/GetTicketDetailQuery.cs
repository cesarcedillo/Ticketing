using MediatR;
using Ticketing.Ticket.Application.Dtos.Responses;

namespace Ticketing.Ticket.Application.Queries.GetTicketDetail;

public sealed record GetTicketDetailQuery(Guid TicketId)
    : IRequest<TicketDetailResponse>;
