using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Diagnostics;
using Ticketing.Core.Observability.OpenTelemetry.Helpers;
using Ticketing.Core.Observability.OpenTelemetry.Options;
using Ticketing.Core.Service.Messenger.Interfaces;
using Ticketing.Core.Service.Messenger.RabbitMQ.Extensions;
using Ticketing.Core.Service.Messenger.RabbitMQ.Interfaces;
using Ticketing.Core.Service.Messenger.Types;

namespace Ticketing.Core.Service.Messenger.RabbitMQ.Services;
public sealed class MessengerReceiveServiceRabbit : IMessengerReceiveService
{
  private readonly IRabbitMQChannel channel;
  private readonly List<string> consumerIds = [];
  private readonly EventingBasicConsumer consumer;
  private readonly string errorsBrokerName;
  private readonly List<string> propertiesToTrace;
  private readonly bool traceContents;
  private readonly ActivitySource? activitySource;

  public event EventHandler<Message>? OnMessageReceived;

  public MessengerReceiveServiceRabbit(
    IRabbitMQChannel channel,
    IOptions<BrokersConfiguration> brokersConfiguration,
    IOptions<OpenTelemetryOptions> openTelemetryOptions,
    ActivitySource? activitySource = null)
  {
    this.channel = channel;
    this.activitySource = activitySource;

    consumer = new EventingBasicConsumer(this.channel.Model);
    consumer.Received += ConsumerMessageReceived;
    consumer.ConsumerCancelled += ConsumerCancelled;
    errorsBrokerName = brokersConfiguration.Value.ErrorsBroker.BrokerName;
    propertiesToTrace = openTelemetryOptions.Value?.PropertiesToTrace ?? [];
    traceContents = openTelemetryOptions.Value?.TraceContents ?? false;
  }

  private void ConsumerCancelled(object? sender, ConsumerEventArgs e)
  {
    UnsubscribeQueues();
  }

  private void ConsumerMessageReceived(object? sender, BasicDeliverEventArgs e)
  {
    try
    {
      Thread.Sleep(GetDelay(e.BasicProperties.GetDeliveryCount()));
      Message message = new(e.Exchange, e.RoutingKey, e.DeliveryTag.ToString(), e.Body)
      {
        OriginalMessageInformacion = ExtendOriginalMessageInformation(e.Exchange, e.BasicProperties),
        TraceID = e.BasicProperties.CorrelationId
      };
      channel.BeginTransaction();
      StartActivityAndProcessMessage(message);
      channel.Commit();
    }
    catch
    {
      channel.RollBack();
    }
  }

  private OriginalMessageInformation? ExtendOriginalMessageInformation(string broker, IBasicProperties basicProperties)
  {
    if (broker.Equals(errorsBrokerName))
    {
      return new(
          basicProperties.GetOriginalExchange(),
          basicProperties.GetOriginalTopic(),
          basicProperties.GetFailureReason()
      );
    }
    return null;
  }

  public void AckMessage(string messageId)
  {
    channel.Ack(messageId);
  }

  public void NackMessage(string messageId)
  {
    channel.Nack(messageId);
  }

  public void SubscribeQueues(List<string> queues)
  {
    ArgumentNullException.ThrowIfNull(queues);
    if (queues.Count == 0)
    {
      throw new InvalidDataException("There is no queue to subscribe to");
    }

    queues.ForEach(queue =>
    {
      consumerIds.Add(channel.BasicConsume(queue, consumer));
    });
  }

  public void UnsubscribeQueues()
  {
    if (consumerIds.Count > 0)
    {
      if (channel.IsOpen)
      {
        consumerIds.ForEach(consumerId =>
        {
          channel.CancelConsume(consumerId);
        });
      }
      consumerIds.Clear();
    }
  }

  private void StartActivityAndProcessMessage(Message message)
  {
    using Activity? activity = activitySource?.StartActivity(name: message.Topic, kind: ActivityKind.Consumer, parentId: message.TraceID);
    activity?.SetActivityCustomProperties("Topic", message.Topic);
    activity?.SetActivityCustomProperties(message.Content, propertiesToTrace);
    if (traceContents)
    {
      activity?.SetActivityCustomProperties("Content", message.Content);
    }
    OnMessageReceived?.Invoke(this, message);
  }

  private int GetDelay(int deliveryCount)
  {
    return deliveryCount switch
    {
      0 => 0,
      1 => 1000,
      2 => 10000,
      3 => 30000,
      _ => 60000 // 1 minute 
    };
  }
}