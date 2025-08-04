namespace Ticketing.Core.Service.Messenger.Types;
public class Event
{
  public Event()
  {
    Queue = "empty-queue";
    Topics = new List<string>();
  }

  public Event(string queue, List<string> topics)
  {
    ArgumentNullException.ThrowIfNull(topics);

    Queue = string.IsNullOrEmpty(queue) ? throw new ArgumentNullException(nameof(queue)) : queue;
    Topics = topics.Count == 0 ? throw new InvalidDataException($"There are no topics in the event") : topics;
  }

  public List<string> Topics { get; set; }
  public string Queue { get; set; }
}