using Microsoft.EntityFrameworkCore;
using Ticketing.Core.Infrastructure.EntityFramework.Repositories;
using Ticketing.Domain.Aggregates;
using Ticketing.Domain.Interfaces.Repositories;
using UserType = Ticketing.Domain.Aggregates.User;

namespace Ticketing.Infrastructure.Data.Repositories;
public class UserRepository : GenericRepository<UserType>, IUserRepository
{
  protected readonly TicketDbContext _dbContext;

  public UserRepository(TicketDbContext dbContext) : base(dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default)
  {
    return await _dbContext.Users
        .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
  }

  public async Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default)
  {
    return await _dbContext.Users
        .FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken);
  }

}
