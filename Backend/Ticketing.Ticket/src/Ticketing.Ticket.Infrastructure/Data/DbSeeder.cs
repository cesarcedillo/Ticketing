using Ticketing.Ticket.Domain.Aggregates;
using Ticketing.Ticket.Domain.Enums;
using TicketType = Ticketing.Ticket.Domain.Aggregates.Ticket;

namespace Ticketing.Ticket.Infrastructure.Data;
public static class DbSeeder
{
  public static async Task SeedAsync(TicketDbContext context)
  {
    //if (!context.Tickets.Any())

    //{
    //  context.Tickets.Add(new TicketType("First ticket", "Ticket dummy", new Guid("")));
    //  context.Tickets.Add(new TicketType("Admin ticket", "Ticket admin", new Guid("")));
    //  await context.SaveChangesAsync();
    //}
  }
}

