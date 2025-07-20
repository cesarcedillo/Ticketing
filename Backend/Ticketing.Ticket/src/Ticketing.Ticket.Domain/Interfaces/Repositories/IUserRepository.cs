using Ticketing.Core.Domain.SeedWork.Interfaces;
using Ticketing.Ticket.Domain.Aggregates;

namespace Ticketing.Ticket.Domain.Interfaces.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
  Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default);

  Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default);

}
