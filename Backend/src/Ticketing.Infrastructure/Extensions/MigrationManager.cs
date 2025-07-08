using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ticketing.Infrastructure.Data;

namespace Ticketing.Infrastructure.Extensions;
  public static class MigrationManager
{
  public static void ApplyMigrations(this IServiceProvider serviceProvider)
  {
    using var scope = serviceProvider.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<TicketDbContext>();
    db.Database.Migrate();
  }
}

