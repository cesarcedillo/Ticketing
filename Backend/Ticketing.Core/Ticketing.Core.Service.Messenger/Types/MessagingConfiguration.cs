namespace Ticketing.Core.Service.Messenger.Types;
public class MessagingConfiguration
{
  public MessagingConfiguration()
  {
    Queues = Enumerable.Empty<Queue>().ToList();
    Events = Enumerable.Empty<Event>().ToList();
  }
  public List<Queue> Queues { get; set; }
  public List<Event> Events { get; set; }

}