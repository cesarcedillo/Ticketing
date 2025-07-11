
namespace Ticketing.Domain.Entities;
public class TicketReply
{
  public Guid Id { get; private set; }
  public string Text { get; private set; } = string.Empty;
  public Guid UserId { get; private set; }
  public User User { get; private set; }
  public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

  private TicketReply() { }

  public TicketReply(string text, User user, DateTime createdAt)
  {
    Id = Guid.NewGuid();
    Text = text ?? throw new ArgumentNullException(nameof(text));
    UserId = user?.Id ?? throw new ArgumentNullException(nameof(user));
    User = user;
    CreatedAt = createdAt;
  }
}
