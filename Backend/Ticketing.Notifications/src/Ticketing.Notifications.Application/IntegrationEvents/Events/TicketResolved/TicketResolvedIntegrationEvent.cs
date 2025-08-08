using Ticketing.Core.Service.IntegrationEvents;

namespace Ticketing.Notifications.Application.IntegrationEvents.Events.TicketResolved;
public sealed record TicketResolvedIntegrationEvent(Guid TicketId, Guid UserId) : IntegrationEvent;
