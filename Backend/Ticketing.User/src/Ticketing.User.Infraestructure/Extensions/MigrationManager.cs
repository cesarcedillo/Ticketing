using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ticketing.User.Infraestructure.Data;

namespace Ticketing.User.Infraestructure.Extensions;
public static class MigrationManager
{
  public static void ApplyMigrations(this IServiceProvider serviceProvider)
  {
    using var scope = serviceProvider.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<UserDbContext>();
    db.Database.Migrate();
  }
}
