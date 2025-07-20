
using System.Text.Json.Serialization;

namespace Ticketing.Ticket.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TicketStatus
{
  Open,
  InResolution,
  Resolved
}