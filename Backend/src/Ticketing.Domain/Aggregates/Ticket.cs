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
  public Guid UserId { get; private set; }
  public User User { get; private set; } = null!;

  private readonly List<TicketReply> _replies = new();
  public IReadOnlyCollection<TicketReply> Replies => _replies.AsReadOnly();

  public Ticket() { }

  public Ticket(string subject, string description, User user)
  {
    Id = Guid.NewGuid();
    Subject = subject ?? throw new ArgumentNullException(nameof(subject));
    Description = description ?? throw new ArgumentNullException(nameof(description));
    Status = TicketStatus.Open;
    UserId = user?.Id ?? throw new ArgumentNullException(nameof(user));
    User = user;
  }

  public void AddReply(TicketReply reply)
  {
    if (Status == TicketStatus.Resolved)
      throw new InvalidOperationException("Ticket is already resolved.");

    _replies.Add(reply);

    if (Status == TicketStatus.Open)
      Status = TicketStatus.InResolution;
  }

  public void MarkAsResolved()
  {
    if (Status == TicketStatus.Resolved)
      throw new InvalidOperationException("Ticket is already resolved.");

    Status = TicketStatus.Resolved;
  }

}

