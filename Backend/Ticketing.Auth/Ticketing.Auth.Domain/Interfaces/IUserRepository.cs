using Ticketing.Auth.Domain.Aggregates;

namespace Ticketing.Auth.Domain.Interfaces;
public interface IUserRepository
{
  Task<User?> GetByUserNameAsync(string username, CancellationToken cancellationToken = default);
  Task AddAsync(User user, CancellationToken cancellationToken = default);
}
