using Ticketing.Application.Dtos.Responses;

namespace Ticketing.Application.Services.Interfaces;
public interface ITicketService
{
  Task<Guid> CreateTicketAsync(string subject, string description, Guid userId, CancellationToken cancellationToken);
  Task<IReadOnlyList<TicketResponse>> ListTicketsAsync(string? status, Guid? userId, CancellationToken cancellationToken);
  Task<TicketDetailResponse?> GetTicketDetailAsync(Guid ticketId, CancellationToken cancellationToken);
  Task MarkTicketAsResolvedAsync(Guid ticketId, CancellationToken cancellationToken);


}
