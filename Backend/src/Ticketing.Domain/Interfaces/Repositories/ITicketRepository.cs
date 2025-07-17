using Ticketing.Core.Domain.SeedWork.Interfaces;
using Ticketing.Domain.Aggregates;

namespace Ticketing.Domain.Interfaces.Repositories;

public interface ITicketRepository : IGenericRepository<Ticket>
{
  Task<Ticket?> GetByIdAsync(Guid ticketId, CancellationToken cancellationToken = default);
  Task<IReadOnlyList<Ticket>> GetUnresolvedTicketsAsync(CancellationToken cancellationToken = default);
  IQueryable<Ticket> Query();
  Task AddReplyAsync(Guid ticketId, string text, Guid userId, CancellationToken cancellationToken);

}
