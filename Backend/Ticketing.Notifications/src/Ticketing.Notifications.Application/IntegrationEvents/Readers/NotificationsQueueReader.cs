using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ticketing.Core.Service.Messenger.Readers;
using Ticketing.Notifications.Application.IntegrationEvents.Events.TicketResolved;
using Ticketing.Notifications.Application.IntegrationEvents.Queues;
using Ticketing.Notifications.Application.IntegrationEvents.Topics;

namespace Ticketing.Notifications.Application.IntegrationEvents.Readers;
  public class NotificationsQueueReader : BaseReader
{
  public override Dictionary<string, Type> EventTypes
  {
    get
    {
      return new Dictionary<string, Type>
                {
                    { IntegrationNotificationsTopics.TicketResolved, typeof(TicketResolvedIntegrationEvent) }
                };
    }
  }
  public override List<string> QueueNames => [NotificationsQueues.Notifications, NotificationsQueues.NotificationsErrores];

  public NotificationsQueueReader(
          IServiceScopeFactory serviceScopeFactory,
          ILogger<NotificationsQueueReader> logger)
      : base(serviceScopeFactory, logger) { }

}
