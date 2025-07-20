using Microsoft.EntityFrameworkCore;
using Ticketing.Core.Infrastructure.EntityFramework.Repositories;
using Ticketing.Ticket.Domain.Aggregates;
using Ticketing.Ticket.Domain.Entities;
using Ticketing.Ticket.Domain.Enums;
using Ticketing.Ticket.Domain.Interfaces.Repositories;
using TicketType = Ticketing.Ticket.Domain.Aggregates.Ticket;

namespace Ticketing.Ticket.Infrastructure.Data.Repositories;
public class TicketRepository : GenericRepository<TicketType>, ITicketRepository
{
  protected readonly TicketDbContext _dbContext;

  public TicketRepository(TicketDbContext dbContext) : base(dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task<TicketType?> GetByIdAsync(Guid ticketId, CancellationToken cancellationToken = default)
  {
    return await _dbContext.Tickets
        .Include(t => t.User)
        .Include(t => t.Replies).ThenInclude(r => r.User)
        .FirstOrDefaultAsync(t => t.Id == ticketId, cancellationToken);
  }

  public async Task<IReadOnlyList<TicketType>> GetUnresolvedTicketsAsync(CancellationToken cancellationToken = default)
  {
    return await _dbContext.Tickets
        .Where(t => t.Status != TicketStatus.Resolved)
        .Include(t => t.User)
        .ToListAsync(cancellationToken);
  }

  public IQueryable<TicketType> Query()
  {
    return _dbContext.Tickets
              .Include(t => t.User)
              .Include(t => t.Replies);
  }

  public async Task AddReplyAsync(Guid ticketId, string text, Guid userId, CancellationToken cancellationToken)
  {
    var ticket = await _dbContext.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId, cancellationToken);
    if (ticket == null)
      throw new KeyNotFoundException($"Ticket {ticketId} not found.");

    var userExists = await _dbContext.Users.AnyAsync(u => u.Id == userId, cancellationToken);
    if (!userExists)
      throw new KeyNotFoundException($"User {userId} not found.");

    var reply = new TicketReply(text, userId, ticketId);
    ticket.MarkAsInResolution();

    _dbContext.TicketReplies.Add(reply);
  }

}
