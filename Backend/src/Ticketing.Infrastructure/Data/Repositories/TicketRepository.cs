using Microsoft.EntityFrameworkCore;
using Ticketing.Core.Infrstructure.EntityFramework.Repositories;
using Ticketing.Domain.Aggregates;
using Ticketing.Domain.Enums;
using Ticketing.Domain.Interfaces.Repositories;
using TicketType = Ticketing.Domain.Aggregates.Ticket;

namespace Ticketing.Infrastructure.Data.Repositories;
public class TicketRepository : GenericRepository<TicketType>, ITicketRepository
{
  protected readonly TicketDbContext _dbContext;

  public TicketRepository(TicketDbContext dbContext) : base(dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task<Ticket?> GetByIdAsync(Guid ticketId, CancellationToken cancellationToken = default)
  {
    return await _dbContext.Tickets
        .Include(t => t.User)
        .Include(t => t.Replies).ThenInclude(r => r.User)
        .FirstOrDefaultAsync(t => t.Id == ticketId, cancellationToken);
  }

  public async Task<IReadOnlyList<Ticket>> GetUnresolvedTicketsAsync(CancellationToken cancellationToken = default)
  {
    return await _dbContext.Tickets
        .Where(t => t.Status != TicketStatus.Resolved)
        .Include(t => t.User)
        .ToListAsync(cancellationToken);
  }


}
