using Ticketing.Core.Domain.SeedWork.Interfaces;
using UserType = Ticketing.User.Domain.Aggregates.User;

namespace Ticketing.User.Domain.Interfaces.Repositories;
public interface IUserRepository : IGenericRepository<UserType>
{
  Task<UserType?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default);

  Task<UserType?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default);
  Task<IEnumerable<UserType>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken);


}