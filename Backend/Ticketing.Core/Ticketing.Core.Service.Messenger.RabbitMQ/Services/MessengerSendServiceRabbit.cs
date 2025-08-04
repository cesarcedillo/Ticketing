using Microsoft.Extensions.Options;
using System.Diagnostics;
using Ticketing.Core.Observability.OpenTelemetry.Helpers;
using Ticketing.Core.Observability.OpenTelemetry.Options;
using Ticketing.Core.Service.Messenger.Interfaces;
using Ticketing.Core.Service.Messenger.RabbitMQ.Interfaces;

namespace Ticketing.Core.Service.Messenger.RabbitMQ.Services;
public sealed class MessengerSendServiceRabbit(
  IRabbitMQChannel channel,
  IMessengerBrokerDiscover messegerBrokerDiscover,
  IOptions<OpenTelemetryOptions> openTelemetryOptions) : IMessengerSendService
{
  private readonly IRabbitMQChannel channel = channel;
  private readonly IMessengerBrokerDiscover messegerBrokerDiscover = messegerBrokerDiscover;
  private readonly List<string> propertiesToTrace = openTelemetryOptions.Value?.PropertiesToTrace ?? [];
  private readonly bool traceContents = openTelemetryOptions.Value?.TraceContents ?? false;

  public void BeginTransaction()
  {
    channel.BeginTransaction();
  }

  public void Commit()
  {
    channel.Commit();
  }

  public void RollBack()
  {
    channel.RollBack();
  }

  public void SendMessage(string body, string topic)
  {
    string exchange = messegerBrokerDiscover.GetBrokerName(topic);
    StartActivityAndPublishMessage(exchange, topic, body);
  }

  private void StartActivityAndPublishMessage(string exchange, string topic, string body)
  {
    using Activity? activity = Activity.Current?.Source.StartActivity(name: topic, kind: ActivityKind.Producer, parentId: Activity.Current.Id);
    activity?.SetActivityCustomProperties("Topic", topic);
    activity?.SetActivityCustomProperties(body, propertiesToTrace);
    if (traceContents)
    {
      activity?.SetActivityCustomProperties("Content", body);
    }
    channel.PublishMessage(exchange: exchange, topic: topic, body: body);
  }
}