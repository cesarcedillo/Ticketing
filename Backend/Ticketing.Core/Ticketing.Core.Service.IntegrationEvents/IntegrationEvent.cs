namespace Ticketing.Core.Service.IntegrationEvents;
public record IntegrationEvent
{
  public DateTime CreationDate { get; set; }

  public IntegrationEvent()
  {
    CreationDate = DateTime.UtcNow;
  }
}