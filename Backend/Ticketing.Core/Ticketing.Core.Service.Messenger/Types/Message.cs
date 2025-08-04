using System.Text;

namespace Ticketing.Core.Service.Messenger.Types;
public record Message
{
  public Message(string origin, string topic, string messageId, string content)
  {
    Origin = string.IsNullOrEmpty(origin) ? throw new ArgumentNullException(nameof(origin)) : origin;
    Topic = string.IsNullOrEmpty(topic) ? throw new ArgumentNullException(nameof(topic)) : topic;
    MessageId = string.IsNullOrEmpty(messageId) ? throw new ArgumentNullException(nameof(messageId)) : messageId;
    Content = string.IsNullOrEmpty(content) ? throw new ArgumentNullException(nameof(content)) : content;
  }

  public Message(string origin, string topic, string messageId, ReadOnlyMemory<byte> content)
      : this(origin, topic, messageId, Encoding.UTF8.GetString(content.Span)) { }
  public string Origin { get; set; }
  public string Topic { get; set; }

  public string MessageId { get; set; }
  public string Content { get; set; }
  public OriginalMessageInformation? OriginalMessageInformacion { get; set; }
  public string TraceID { get; set; }
}