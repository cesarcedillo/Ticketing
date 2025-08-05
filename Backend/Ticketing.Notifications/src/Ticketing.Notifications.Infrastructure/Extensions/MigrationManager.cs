using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ticketing.Notifications.Infrastructure.Data;

namespace Ticketing.Notifications.Infrastructure.Extensions;
public static class MigrationManager
{
  public static void ApplyMigrations(this IServiceProvider serviceProvider)
  {
    using var scope = serviceProvider.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<NotificationsDbContext>();
    db.Database.Migrate();
  }
}
