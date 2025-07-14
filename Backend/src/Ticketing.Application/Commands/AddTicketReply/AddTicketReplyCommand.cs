using MediatR;

namespace Ticketing.Application.Commands.AddTicketReply;

public sealed record AddTicketReplyCommand(Guid TicketId, string Text, Guid UserId)
    : IRequest<Guid>;
