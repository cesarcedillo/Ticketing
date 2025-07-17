
using Microsoft.EntityFrameworkCore;
using Ticketing.Core.Infrstructure.EntityFramework.Context;
using Ticketing.Domain.Aggregates;
using Ticketing.Domain.Entities;
using Ticketing.Infrastructure.EntityConfigurations;

namespace Ticketing.Infrastructure.Data;

public class TicketDbContext : DbContextBase
{

  public virtual DbSet<Ticket> Tickets => Set<Ticket>(); 
  public virtual DbSet<TicketReply> TicketReplies => Set<TicketReply>();
  public virtual DbSet<User> Users => Set<User>();

  public TicketDbContext(DbContextOptions<TicketDbContext> options) : base(options) { }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {

    modelBuilder.ApplyConfiguration(new UserConfiguration());
    modelBuilder.ApplyConfiguration(new TicketConfiguration());
    modelBuilder.ApplyConfiguration(new TicketReplyConfiguration());

    base.OnModelCreating(modelBuilder);
  }

  public override async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
  {
    await SaveChangesAsync(cancellationToken);
    return true;
  }
}