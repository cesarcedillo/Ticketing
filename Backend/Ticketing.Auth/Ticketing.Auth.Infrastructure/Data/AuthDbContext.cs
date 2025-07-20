using Microsoft.EntityFrameworkCore;
using Ticketing.Auth.Domain.Aggregates;
using Ticketing.Auth.Infrastructure.EntityConfigurations;
using Ticketing.Core.Infrastructure.EntityFramework.Context;

namespace Ticketing.Auth.Infrastructure.Data;
public class AuthDbContext : DbContextBase
{
  public virtual DbSet<User> Users => Set<User>();

  public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

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