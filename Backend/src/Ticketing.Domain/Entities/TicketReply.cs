
using Ticketing.Domain.Aggregates;

namespace Ticketing.Domain.Entities;
public class TicketReply
{
  public Guid Id { get; private set; }
  public string Text { get; private set; }
  public Guid UserId { get; private set; }
  public User User { get; private set; }
  public DateTime CreatedAt { get; private set; }
  public Guid TicketId { get; private set; }
  public Ticket Ticket { get; private set; }

  private TicketReply() { }

  public TicketReply(string text, User user, Ticket ticket, DateTime createdAt)
  {
    Id = Guid.NewGuid();
    Text = text ?? throw new ArgumentNullException(nameof(text));
    UserId = user?.Id ?? throw new ArgumentNullException(nameof(user));
    User = user;
    TicketId = ticket?.Id ?? throw new ArgumentNullException(nameof(ticket));
    Ticket = ticket;
    CreatedAt = createdAt;
  }
}

