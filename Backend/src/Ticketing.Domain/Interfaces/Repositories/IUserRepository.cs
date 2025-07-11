using Ticketing.Core.Domain.SeedWork.Interfaces;
using Ticketing.Domain.Aggregates;

namespace Ticketing.Domain.Interfaces.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
  Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default);

}
