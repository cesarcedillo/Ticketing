
namespace Ticketing.Notifications.Application.IntegrationEvents;
public record IntegrationEvent
{
  public DateTime CreationDate { get; set; }

  public IntegrationEvent()
  {
    CreationDate = DateTime.UtcNow;
  }
}