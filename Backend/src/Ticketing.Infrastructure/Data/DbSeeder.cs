using Ticketing.Domain.Aggregates;
using Ticketing.Domain.Enums;

namespace Ticketing.Infrastructure.Data;
public static class DbSeeder
{
  public static async Task SeedAsync(TicketDbContext context)
  {
    if (!context.Users.Any())
    {
      var admin = new User("admin", "iVBORw0KGgoAAAANSUhEUgAAACgAAAAoCAIAAAADnC86AAAAO0lEQVRIDbXBAQ0AAAgCoNm/9HI4BlFq8g5nZs0Rm81Rm81Rm81Rm81Rm81Rm81Rm81Rm81RkFMAF6YB3e+eU7UAAAAASUVORK5CYII=", UserType.Admin);
      var alice = new User("alice", "iVBORw0KGgoAAAANSUhEUgAAACgAAAAoCAIAAAADnC86AAAAOElEQVRIDbXBAQ0AAAgCoNm/9HI4BlFq8g5nZs0Rm81Rm81Rm81Rm81Rm81Rm81Rm81Rm81RlVMAF3jB1nByd6AAAAASUVORK5CYII=", UserType.Customer);

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

