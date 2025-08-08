using RabbitMQ.Client;
using System.Text;
using System.Text.RegularExpressions;
using Ticketing.Core.Service.Messenger.Types;

namespace Ticketing.Core.Service.Messenger.RabbitMQ.Extensions;
public static class RabbitMQExtensions
{
  private const string HEADER_DELIVERY_COUNT = "x-delivery-count";
  private const string HEADER_DEATH_EXCHANGE = "x-first-death-exchange";
  private const string HEADER_DEATH_QUEUE = "x-first-death-queue";
  private const string HEADER_DEATH_REASON = "x-first-death-reason";
  private const string HEADER_DEATH = "x-death";
  private const string ROUTING_KEYS_KEY = "routing-keys";

  internal static int GetDeliveryCount(this IBasicProperties basicProperties)
  {
    return int.TryParse(basicProperties.GetHeaderProperty(HEADER_DELIVERY_COUNT).ToString(), out int result) ? result : 0;
  }
  internal static string GetOriginalExchange(this IBasicProperties basicProperties)
  {
    return GetContent(basicProperties.GetHeaderProperty(HEADER_DEATH_EXCHANGE));
  }

  internal static string GetOriginalQueue(this IBasicProperties basicProperties)
  {
    return GetContent(basicProperties.GetHeaderProperty(HEADER_DEATH_QUEUE));
  }

  public static string GetOriginalTopic(this IBasicProperties basicProperties)
  {
    var xDeathHeaders = basicProperties.Headers.GetValueOrDefault(HEADER_DEATH) as List<object>;
    var xDeathHeaderDictionary = xDeathHeaders?.FirstOrDefault() as Dictionary<string, object>;
    var routingKeys = xDeathHeaderDictionary?.GetValueOrDefault(ROUTING_KEYS_KEY) as List<object>;
    var originalTopic = GetContent(routingKeys?.FirstOrDefault());
    return originalTopic;
  }
  private static TObject? GetValueOrDefault<TKey, TObject>(this IDictionary<TKey, TObject> dictionary, TKey key)
  {
    return dictionary.TryGetValue(key, out var ret) ? ret : default;
  }
  internal static string GetFailureReason(this IBasicProperties basicProperties)
  {
    return GetContent(basicProperties.GetHeaderProperty(HEADER_DEATH_REASON));
  }

  private static object GetHeaderProperty(this IBasicProperties e, string headerPropertyName)
  {
    if (e.Headers == null)
    {
      return "";
    }
    else if (!e.Headers.ContainsKey(headerPropertyName))
    {
      return "";
    }
    else
    {
      return e.Headers[headerPropertyName] ?? "";
    }
  }

  private static string GetContent(object? content)
  {
    if (content is byte[] bytesContent)
    {
      return Encoding.UTF8.GetString(bytesContent);
    }

    return "";
  }

  internal static void CreateExchanges(this IModel model, BrokersConfiguration brokersConfiguration)
  {
    brokersConfiguration.AllBrokers.ForEach(broker =>
          model.ExchangeDeclare(exchange: broker.BrokerName, type: ExchangeType.Topic)
      );
  }

  internal static void CreateQueues(this IModel model, BrokersConfiguration brokersConfiguration, MessagingConfiguration messagingConfiguration)
  {
    messagingConfiguration.Queues.ForEach(queue =>
    {
      IDictionary<string, object> queueArgs = new Dictionary<string, object>
          {
              { Headers.XQueueType, "quorum" },
              { "delay", queue.DelayBetweenRetries }
          };

      if (queue.MaxRetries > 0 && !string.Equals(queue.ErrorTopic, "errores.empty-queue", StringComparison.OrdinalIgnoreCase))
      {
        queueArgs["x-delivery-limit"] = queue.MaxRetries;
        queueArgs["x-dead-letter-exchange"] = brokersConfiguration.ErrorsBroker.BrokerName;
        queueArgs["x-dead-letter-routing-key"] = queue.ErrorTopic;
      }

      model.QueueDeclare(queue: queue.Name, arguments: queueArgs, autoDelete: false, exclusive: false, durable: true);
    });
  }

  internal static void BindQueues(this IModel model, BrokersConfiguration brokersConfiguration, MessagingConfiguration messagingConfiguration)
  {
    messagingConfiguration.Events.ForEach(queueEvent =>
        queueEvent.Topics.ForEach(topic =>
        {
          string topicRegex = $"^{topic.Replace(".", "\\.").Replace("*", ".*")}$";
          brokersConfiguration.AllBrokers
                  .FindAll(broker => broker.Topics.Exists(t => Regex.IsMatch(t, topicRegex, RegexOptions.IgnoreCase, TimeSpan.FromSeconds(5))))
                  .ForEach(broker => model.QueueBind(queue: queueEvent.Queue, exchange: broker.BrokerName, routingKey: topic, arguments: null));

        })
    );
  }
}


