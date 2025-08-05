using Ticketing.Notifications.Domain.Aggregates;

namespace Ticketing.Notifications.Domain.Interfaces;
public interface INotificationRepository
{
  Task<IReadOnlyList<Notification>> GetByRecipientAsync(string recipient, CancellationToken cancellationToken = default);

  Task<Notification?> GetByIdAsync(Guid notificationId, CancellationToken cancellationToken = default);
}
