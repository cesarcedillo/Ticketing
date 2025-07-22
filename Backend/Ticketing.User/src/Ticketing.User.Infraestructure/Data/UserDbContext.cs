using Microsoft.EntityFrameworkCore;
using Ticketing.Core.Infrastructure.EntityFramework.Context;
using Ticketing.User.Infraestructure.EntityConfigurations;
using UserType = Ticketing.User.Domain.Aggregates.User;

namespace Ticketing.User.Infraestructure.Data;
public class UserDbContext : DbContextBase
{
  public virtual DbSet<UserType> Users => Set<UserType>();

  public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {

    modelBuilder.ApplyConfiguration(new UserConfiguration());

    base.OnModelCreating(modelBuilder);
  }

  public override async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
  {
    await SaveChangesAsync(cancellationToken);
    return true;
  }
}
