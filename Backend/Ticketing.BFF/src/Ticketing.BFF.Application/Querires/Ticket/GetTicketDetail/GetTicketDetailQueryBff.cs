using MediatR;
using Ticketing.BFF.Application.Dto.Responses;

namespace Ticketing.BFF.Application.Querires.Ticket.GetTicketDetail;
public sealed record GetTicketDetailQueryBff(Guid TicketId) : IRequest<TicketDetailResponseBff>;