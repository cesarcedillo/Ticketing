
using Microsoft.EntityFrameworkCore;
using Ticketing.Core.Infrastructure.EntityFramework.Context;
using Ticketing.Ticket.Domain.Aggregates;
using Ticketing.Ticket.Domain.Entities;
using Ticketing.Ticket.Infrastructure.EntityConfigurations;
using TicketType = Ticketing.Ticket.Domain.Aggregates.Ticket;

namespace Ticketing.Ticket.Infrastructure.Data;

public class TicketDbContext : DbContextBase
{

  public virtual DbSet<TicketType> Tickets => Set<TicketType>(); 
  public virtual DbSet<TicketReply> TicketReplies => Set<TicketReply>();

  public TicketDbContext(DbContextOptions<TicketDbContext> options) : base(options) { }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
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