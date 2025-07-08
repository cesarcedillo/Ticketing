using Ticketing.Core.Domain.SeedWork.Interfaces;

namespace Ticketing.Domain.Interfaces.Repositories;

public interface ITicketRepository : IGenericRepository<Aggregates.Ticket>
{

}
