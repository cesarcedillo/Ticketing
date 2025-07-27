using Ticketing.Ticket.Application.Dtos.Responses;

namespace Ticketing.Ticket.Application.Services.Interfaces;
public interface ITicketService
{
  Task<Guid> CreateTicketAsync(string subject, string description, Guid userId, CancellationToken cancellationToken);
  Task<IReadOnlyList<TicketResponse>> ListTicketsAsync(string? status, Guid? userId, CancellationToken cancellationToken);
  Task<TicketDetailResponse> GetTicketDetailAsync(Guid ticketId, CancellationToken cancellationToken);
  Task MarkTicketAsResolvedAsync(Guid ticketId, CancellationToken cancellationToken);
  Task AddReplyAsync(Guid ticketId, string text, Guid userId, CancellationToken cancellationToken);

}
