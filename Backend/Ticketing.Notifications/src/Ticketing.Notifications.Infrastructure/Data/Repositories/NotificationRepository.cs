using Microsoft.EntityFrameworkCore;
using Ticketing.Core.Infrastructure.EntityFramework.Repositories;
using Ticketing.Notifications.Domain.Aggregates;
using Ticketing.Notifications.Domain.Interfaces;

namespace Ticketing.Notifications.Infrastructure.Data.Repositories;
public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
{
  private readonly NotificationsDbContext _context;

  public NotificationRepository(NotificationsDbContext context) : base(context)
  {
    _context = context;
  }

  public async Task<Notification?> GetByIdAsync(Guid notificationId, CancellationToken cancellationToken = default)
  {
    return await _context.Notifications
        .FirstOrDefaultAsync(n => n.Id == notificationId, cancellationToken);
  }

  public async Task<IReadOnlyList<Notification>> GetByRecipientAsync(string recipient, CancellationToken cancellationToken = default)
  {
    return await _context.Notifications
        .Where(n => n.Recipient == recipient)
        .OrderByDescending(n => n.CreatedAt)
        .ToListAsync(cancellationToken);
  }
}
