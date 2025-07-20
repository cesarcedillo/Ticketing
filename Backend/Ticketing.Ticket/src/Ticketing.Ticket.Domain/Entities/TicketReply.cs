
using Ticketing.Ticket.Domain.Aggregates;
using TicketType = Ticketing.Ticket.Domain.Aggregates.Ticket;

namespace Ticketing.Ticket.Domain.Entities;
public class TicketReply
{
  public Guid Id { get; private set; }
  public string Text { get; private set; }
  public Guid UserId { get; private set; }
  public User User { get; private set; }
  public DateTime CreatedAt { get; private set; }
  public Guid TicketId { get; private set; }
  public TicketType Ticket { get; private set; }

  private TicketReply() { }

  public TicketReply(string text, User user, TicketType ticket)
  {

    if (string.IsNullOrEmpty(text))
      throw new ArgumentException("Text cannot be empty.", nameof(text));

    Id = Guid.NewGuid();
    Text = text;
    UserId = user?.Id ?? throw new ArgumentNullException(nameof(user));
    User = user;
    TicketId = ticket?.Id ?? throw new ArgumentNullException(nameof(ticket));
    Ticket = ticket;
    CreatedAt = DateTime.UtcNow;
  }

  public TicketReply(string text, Guid userId, Guid ticketId)
  {
    if (string.IsNullOrEmpty(text))
      throw new ArgumentException("Text cannot be empty.", nameof(text));

    Id = Guid.NewGuid();
    Text = text;
    UserId = userId;
    TicketId = ticketId;
    CreatedAt = DateTime.UtcNow;
  }

}

