
using Microsoft.EntityFrameworkCore;
using Ticketing.Core.Infrstructure.EntityFramework.Context;
using Ticketing.Infrastructure.EntityConfigurations;

namespace Ticketing.Infrastructure.Data;

public class TicketDbContext : DbContextBase
{

  public virtual DbSet<Domain.Aggregates.Ticket> Tickets => Set<Domain.Aggregates.Ticket>();

  public TicketDbContext(DbContextOptions<TicketDbContext> options) : base(options) { }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {

    modelBuilder.ApplyConfiguration(new TicketConfiguration());

    base.OnModelCreating(modelBuilder);
  }

  public override async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
  {
    await SaveChangesAsync(cancellationToken);
    return true;
  }
}