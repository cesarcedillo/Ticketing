using Ticketing.Notifications.Application.Services.Interfaces;

namespace Ticketing.Notifications.Application.Services;
public class NotificationsService : INotificationsService
{
  public Task NotifyTicketResolvedAsync(Guid ticketId, Guid userId, CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }
}