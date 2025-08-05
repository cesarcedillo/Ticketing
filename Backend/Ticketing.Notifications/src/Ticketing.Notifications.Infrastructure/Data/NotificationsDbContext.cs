using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Core.Infrastructure.EntityFramework.Context;
using Ticketing.Notifications.Domain.Aggregates;
using Ticketing.Notifications.Infrastructure.EntityConfigurations;

namespace Ticketing.Notifications.Infrastructure.Data;
public class NotificationsDbContext : DbContextBase
{
  public DbSet<Notification> Notifications { get; set; }

  public NotificationsDbContext(DbContextOptions<NotificationsDbContext> options)
  : base(options)
  {
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {

    modelBuilder.ApplyConfiguration(new NotificationConfiguration());

    base.OnModelCreating(modelBuilder);
  }

  public override async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
  {
    await SaveChangesAsync(cancellationToken);
    return true;
  }
}
