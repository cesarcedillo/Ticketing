using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ticketing.Auth.Infrastructure.Data;

namespace Ticketing.Auth.Infrastructure.Extensions;
  public static class MigrationManager
{
  public static void ApplyMigrations(this IServiceProvider serviceProvider)
  {
    using var scope = serviceProvider.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
    db.Database.Migrate();
  }
}

