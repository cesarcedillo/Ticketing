
using Microsoft.EntityFrameworkCore;
using Ticketing.Core.Infrastructure.EntityFramework.Repositories;
using Ticketing.User.Domain.Interfaces.Repositories;
using UserType = Ticketing.User.Domain.Aggregates.User;

namespace Ticketing.User.Infrastructure.Data.Repositories;
public class UserRepository : GenericRepository<UserType>, IUserRepository
{
  protected readonly UserDbContext _dbContext;

  public UserRepository(UserDbContext dbContext) : base(dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task<UserType?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default)
  {
    return await _dbContext.Users
        .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
  }

  public async Task<UserType?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default)
  {
    return await _dbContext.Users
        .FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken);
  }

  public async Task<IEnumerable<UserType>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
  {
    if (ids == null || !ids.Any())
      return Enumerable.Empty<UserType>();

    return await _dbContext.Users
        .Where(u => ids.Contains(u.Id))
        .ToListAsync(cancellationToken);
  }


}