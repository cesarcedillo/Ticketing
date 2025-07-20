using Microsoft.EntityFrameworkCore;
using Ticketing.Auth.Domain.Aggregates;
using Ticketing.Auth.Domain.Interfaces;

namespace Ticketing.Auth.Infrastructure.Data.Repositories;
public class UserRepository : IUserRepository
{
  private readonly AuthDbContext _dbContext;

  public UserRepository(AuthDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken)
  {
    return await _dbContext.Users
        .AsNoTracking()
        .FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken);
  }

  public async Task AddAsync(User user, CancellationToken cancellationToken)
  {
    _dbContext.Users.Add(user);
    await _dbContext.SaveChangesAsync(cancellationToken);
  }

}
