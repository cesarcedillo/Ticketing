using Ticket.Cliente.NswagAutoGen.HttpClientFactoryImplementation;
using Ticketing.BFF.Application.Dto.Responses;

namespace Ticketing.BFF.Application.Services.Interfaces;
public interface ITicketService
{
  Task<IReadOnlyList<TicketResponseBff>> ListTicketsAsync(string? status, Guid? userId, CancellationToken cancellationToken);
  Task<TicketDetailResponseBff> GetTicketDetailAsync(Guid ticketId, CancellationToken cancellationToken);
  Task<Guid> CreateTicketAsync(string subject, string description, Guid userId, CancellationToken cancellationToken);
  Task AddReplyAsync(Guid ticketId, string text, Guid userId, CancellationToken cancellationToken);
  Task MarkTicketAsResolvedAsync(Guid ticketId, CancellationToken cancellationToken);
}
