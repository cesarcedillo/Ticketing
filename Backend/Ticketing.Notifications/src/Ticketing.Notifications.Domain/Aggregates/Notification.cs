using Ticketing.Core.Domain.SeedWork.Interfaces;
using Ticketing.Notifications.Domain.Enums;

namespace Ticketing.Notifications.Domain.Aggregates;
public class Notification : IAggregateRoot
{
  public Guid Id { get; private set; }
  public string Title { get; private set; }
  public string Message { get; private set; }
  public string Recipient { get; private set; } // Email or UserId
  public NotificationType Type { get; private set; }
  public DateTimeOffset CreatedAt { get; private set; }
  public bool IsRead { get; private set; }

  private Notification() { }

  public Notification(string title, string message, string recipient, NotificationType type)
  {
    Id = Guid.NewGuid();
    Title = title;
    Message = message;
    Recipient = recipient;
    Type = type;
    CreatedAt = DateTimeOffset.UtcNow;
    IsRead = false;
  }

  public void MarkAsRead()
  {
    if (Type == NotificationType.Front)
      IsRead = true;
  }
}
