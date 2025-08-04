namespace Ticketing.Core.Service.Messenger.Types;
public class Queue
{
  public const int DEFAULT_QUEUE_MAX_RETRIES = 3;
  public const int DEFAULT_QUEUE_DELAY = 2000;

  public Queue() : this("empty-queue") { }
  public Queue(string name) : this(name, DEFAULT_QUEUE_MAX_RETRIES, DEFAULT_QUEUE_DELAY, $"errores.{name}") { }
  public Queue(string name, int maxRetries, int delayBetweenRetries, string errorTopic)
  {
    Name = string.IsNullOrEmpty(name) ? throw new ArgumentNullException(nameof(name)) : name;
    ErrorTopic = string.IsNullOrEmpty(errorTopic) ? throw new ArgumentNullException(nameof(errorTopic)) : errorTopic;

    MaxRetries = maxRetries >= 0 && maxRetries <= 10 ? maxRetries : throw new ArgumentOutOfRangeException(nameof(maxRetries), "MaxRetries must be between 0 and 10");
    DelayBetweenRetries = delayBetweenRetries >= 0 && delayBetweenRetries <= 10000 ? delayBetweenRetries : throw new ArgumentOutOfRangeException(nameof(delayBetweenRetries), "DelayBetweenRetries must be between 0 and 10000");
  }


  public string Name { get; set; }
  public int MaxRetries { get; set; }
  public string ErrorTopic { get; set; }
  public int DelayBetweenRetries { get; set; }
}