using Ticketing.Core.Domain.SeedWork.Interfaces;
using TicketType = Ticketing.Ticket.Domain.Aggregates.Ticket;

namespace Ticketing.Ticket.Domain.Interfaces.Repositories;

public interface ITicketRepository : IGenericRepository<TicketType>
{
  Task<TicketType?> GetByIdAsync(Guid ticketId, CancellationToken cancellationToken = default);
  Task<IReadOnlyList<TicketType>> GetUnresolvedTicketsAsync(CancellationToken cancellationToken = default);
  IQueryable<TicketType> Query();
  Task AddReplyAsync(Guid ticketId, string text, Guid userId, CancellationToken cancellationToken);

}
