
using System.Text.Json.Serialization;

namespace Ticketing.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TicketStatus
{
  Open,
  InResolution,
  Resolved
}