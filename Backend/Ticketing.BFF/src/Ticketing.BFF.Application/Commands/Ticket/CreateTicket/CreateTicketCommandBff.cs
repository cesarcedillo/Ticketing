using MediatR;
using Ticketing.BFF.Application.Dto.Responses;

namespace Ticketing.BFF.Application.Commands.Ticket.CreateTicket;
public sealed record CreateTicketCommandBff(string Subject, string Description, Guid UserId)
    : IRequest<CreateTicketResponseBff>;