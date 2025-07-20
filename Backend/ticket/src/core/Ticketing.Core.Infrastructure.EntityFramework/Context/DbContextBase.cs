using Microsoft.EntityFrameworkCore;
using Ticketing.Core.Domain.SeedWork.Interfaces;

namespace Ticketing.Core.Infrastructure.EntityFramework.Context;
public class DbContextBase : DbContext, IUnitOfWork
{
  public DbContextBase() { }

  public DbContextBase(DbContextOptions options) : base(options)
  {
  }

  public virtual async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
  {
    await SaveChangesAsync(cancellationToken);
    return true;
  }

  public void ClearChangeTracker()
  {
    ChangeTracker?.Clear();
  }
}
