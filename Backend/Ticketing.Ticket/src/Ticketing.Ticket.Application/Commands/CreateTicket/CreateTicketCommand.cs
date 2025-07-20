using MediatR;
using Ticketing.Ticket.Application.Dtos;

namespace Ticketing.Ticket.Application.Commands.CreateTicket;
public sealed record CreateTicketCommand(string Subject, string Description, Guid UserId)
    : IRequest<CreateTicketResponse>;


