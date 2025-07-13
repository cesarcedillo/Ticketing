using Ticketing.Domain.Aggregates;
using Ticketing.Domain.Enums;
using Ticketing.TestCommon.Builders;

namespace Ticketing.TestCommon.Fixtures;

  public class DomainFixture
{
  public User CreateDefaultCustomer(string? username = null)
  {
    return new UserBuilder()
        .WithUsername(username ?? "customer")
        .WithUserType(UserType.Customer)
        .Build();
  }

  public User CreateDefaultAgent(string? username = null)
  {
    return new UserBuilder()
        .WithUsername(username ?? "agent")
        .WithUserType(UserType.Agent)
        .Build();
  }

  public Ticket CreateTicketWithReplies(
      string subject = "Default Subject",
      string description = "Default Description",
      int repliesCount = 1)
  {
    var customer = CreateDefaultCustomer();
    var agent = CreateDefaultAgent();
    var ticket = new TicketBuilder()
        .WithSubject(subject)
        .WithDescription(description)
        .WithUser(customer)
        .Build();

    for (int i = 0; i < repliesCount; i++)
    {
      var reply = new TicketReplyBuilder()
          .WithText($"Reply {i + 1}")
          .WithUser(agent)
          .WithTicket(ticket)
          .Build();

      ticket.AddReply(reply);
    }

    return ticket;
  }

  public IEnumerable<Ticket> CreateMultipleTickets(int count)
  {
    var tickets = new List<Ticket>();
    for (int i = 0; i < count; i++)
    {
      tickets.Add(new TicketBuilder()
          .WithSubject($"Subject {i + 1}")
          .WithDescription($"Description {i + 1}")
          .Build());
    }
    return tickets;
  }
}

