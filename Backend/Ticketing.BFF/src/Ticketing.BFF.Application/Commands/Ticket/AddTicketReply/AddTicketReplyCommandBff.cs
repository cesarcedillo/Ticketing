using MediatR;

namespace Ticketing.BFF.Application.Commands.Ticket.AddTicketReply;
public sealed record AddTicketReplyCommandBff(Guid TicketId, string Text, Guid UserId) : IRequest;
