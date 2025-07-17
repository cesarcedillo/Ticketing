using Ticketing.Domain.Aggregates;
using Ticketing.Domain.Enums;

namespace Ticketing.Infrastructure.Data;
public static class DbSeeder
{
  public static async Task SeedAsync(TicketDbContext context)
  {
    if (!context.Users.Any())
    {
      var admin = new User("admin", [1, 2, 3 ], UserType.Admin);
      var alice = new User("alice", [4, 5, 6 ], UserType.Customer);

      context.Users.AddRange(admin, alice);
      await context.SaveChangesAsync();
    }

    if (!context.Tickets.Any())
    {
      var admin = context.Users.FirstOrDefault(u => u.UserName == "admin");
      var alice = context.Users.FirstOrDefault(u => u.UserName == "alice");

      if (admin != null && alice != null)
      {
        context.Tickets.Add(new Ticket("First ticket", "Ticket dummy", alice));
        context.Tickets.Add(new Ticket("Admin ticket", "Ticket admin", admin));
        await context.SaveChangesAsync();
      }
    }
  }
}

