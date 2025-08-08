namespace Ticketing.Notifications.Application.Services.Interfaces;
public interface INotificationsService
{
  Task NotifyTicketResolvedAsync(Guid ticketId, Guid userId, CancellationToken cancellationToken);

}