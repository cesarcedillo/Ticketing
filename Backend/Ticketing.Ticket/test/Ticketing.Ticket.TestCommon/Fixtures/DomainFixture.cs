using Ticketing.Ticket.Domain.Aggregates;
using Ticketing.Ticket.Domain.Enums;
using Ticketing.Ticket.TestCommon.Builders;
using TicketType = Ticketing.Ticket.Domain.Aggregates.Ticket;

namespace Ticketing.Ticket.TestCommon.Fixtures;

  public class DomainFixture
{
  public TicketType CreateTicketWithReplies(
      string subject = "Default Subject",
      string description = "Default Description",
      int repliesCount = 1)
  {

    var ticket = new TicketBuilder()
        .WithSubject(subject)
        .WithDescription(description)
        .Build();

    for (int i = 0; i < repliesCount; i++)
    {
      var reply = new TicketReplyBuilder()
          .WithText($"Reply {i + 1}")
          .WithTicket(ticket)
          .Build();

      ticket.AddReply(reply);
    }

    return ticket;
  }

  public IEnumerable<TicketType> CreateMultipleTickets(int count)
  {
    var tickets = new List<TicketType>();
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

