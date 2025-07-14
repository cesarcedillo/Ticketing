using MediatR;
using Ticketing.Application.Dtos;

namespace Ticketing.Application.Commands.CreateTicket;
public sealed record CreateTicketCommand(string Subject, string Description, Guid UserId)
    : IRequest<CreateTicketResponse>;


