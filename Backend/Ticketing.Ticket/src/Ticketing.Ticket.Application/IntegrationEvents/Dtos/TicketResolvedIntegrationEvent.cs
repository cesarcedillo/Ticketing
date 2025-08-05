namespace Ticketing.Ticket.Application.IntegrationEvents.Dtos;
public sealed record TicketResolvedIntegrationEvent(Guid TicketId, Guid UserId);
