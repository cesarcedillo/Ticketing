using MediatR;

namespace Ticketing.Ticket.Application.Commands.AddTicketReply;

public sealed record AddTicketReplyCommand(Guid TicketId, string Text, Guid UserId)
    : IRequest;
