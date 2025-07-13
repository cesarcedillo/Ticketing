using Ticketing.Core.Domain.SeedWork.Interfaces;
using Ticketing.Domain.Entities;
using Ticketing.Domain.Enums;

namespace Ticketing.Domain.Aggregates;

public class Ticket : IAggregateRoot
{
  public Guid Id { get; private set; }
  public string Subject { get; private set; } = string.Empty;
  public string Description { get; private set; } = string.Empty;
  public TicketStatus Status { get; private set; }
  public DateTime CreatedAt { get; private set; }
  public Guid UserId { get; private set; }
  public User User { get; private set; } = null!;

  private readonly List<TicketReply> _replies = new();
  public IReadOnlyCollection<TicketReply> Replies => _replies.AsReadOnly();

  public Ticket() { }

  public Ticket(string subject, string description, User user)
  {
    if (string.IsNullOrWhiteSpace(subject))
      throw new ArgumentException("Subject cannot be empty.", nameof(subject));

    if (string.IsNullOrWhiteSpace(description))
      throw new ArgumentException("Description cannot be empty.", nameof(description));

    User = user ?? throw new ArgumentNullException(nameof(user), "User cannot be null.");
    UserId = user.Id;
    Id = Guid.NewGuid();
    Subject = subject;
    Description = description;
    Status = TicketStatus.Open;
    CreatedAt = DateTime.UtcNow;
  }

  public void AddReply(TicketReply reply)
  {
    if (reply == null)
      throw new ArgumentNullException(nameof(reply), "Reply cannot be null.");

    if (Status == TicketStatus.Resolved)
      throw new InvalidOperationException("Cannot add reply to a resolved ticket.");

    if (_replies.Any(r => r.Id == reply.Id))
      throw new InvalidOperationException("This reply has already been added.");

    if (Status == TicketStatus.Open)
      Status = TicketStatus.InResolution;

    _replies.Add(reply);
  }


  public void MarkAsResolved()
  {
    if (Status == TicketStatus.Resolved)
      throw new InvalidOperationException("Ticket is already resolved.");

    Status = TicketStatus.Resolved;
  }

}

