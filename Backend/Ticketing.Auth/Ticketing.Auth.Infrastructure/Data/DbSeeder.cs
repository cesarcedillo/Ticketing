using Ticketing.Auth.Domain.Aggregates;
using Ticketing.Auth.Domain.Enum;
using Ticketing.Auth.Infrastructure.Services;

namespace Ticketing.Auth.Infrastructure.Data;
public static class DbSeeder
{
  public static async Task SeedAsync(AuthDbContext context)
  {
    if (!context.Users.Any())
    {

      var passwordHasher = new SimplePasswordHasher();

      var users = new List<User>
            {
                new User("admin", passwordHasher.Hash("1234"), Role.Admin),
                new User("alice", passwordHasher.Hash("1234"), Role.Customer),
                new User("bob", passwordHasher.Hash("1234"), Role.Agent)
            };

      context.Users.AddRange(users);
      await context.SaveChangesAsync();
    }
  }
}
