using Ticketing.Core.Infrstructure.EntityFramework.Repositories;
using Ticketing.Domain.Interfaces.Repositories;
using TicketType = Ticketing.Domain.Aggregates.Ticket;

namespace Ticketing.Infrastructure.Data.Repositories;
public class TicketRepository : GenericRepository<TicketType>, ITicketRepository
{
  protected readonly TicketDbContext _ticketDbContext;

  public TicketRepository(TicketDbContext ticketDbContext) : base(ticketDbContext)
  {
    _ticketDbContext = ticketDbContext;
  }

}
