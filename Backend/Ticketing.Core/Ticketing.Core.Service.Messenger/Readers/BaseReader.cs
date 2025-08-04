using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using System.Text.Json;
using Ticketing.Core.Service.IntegrationEvents;
using Ticketing.Core.Service.Messenger.Interfaces;
using Ticketing.Core.Service.Messenger.Types;

namespace Ticketing.Core.Service.Messenger.Readers;
public abstract class BaseReader : BackgroundService
{
  protected BaseReader(IServiceScopeFactory serviceScopeFactory, ILogger<BaseReader> logger)
  {
    _serviceScopeFactory = serviceScopeFactory;
    _logger = logger;
    _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };
  }

  private readonly IServiceScopeFactory _serviceScopeFactory;
  private readonly ILogger<BaseReader> _logger;
  private readonly JsonSerializerOptions _jsonSerializerOptions;

  private IMessengerReceiveService _messengerReceiveService = null!;

  public abstract Dictionary<string, Type> EventTypes { get; }
  public virtual List<string> QueueNames => [];

  public virtual void OnMessageReceivedEvent(object? sender, Message message)
  {
    try
    {
      if (!EventTypes.TryGetValue(message.Topic, out var integrationEventType))
      {
        throw new InvalidOperationException("The message type is not recognized");
      }

      using var localScope = _serviceScopeFactory.CreateScope();
      foreach (var handler in localScope.ServiceProvider.GetKeyedServices<IIntegrationEventHandler>(integrationEventType))
      {
        RunIntegrationEventHandler(message, integrationEventType, handler);
        _messengerReceiveService.AckMessage(message.MessageId);
      }
    }
    catch (MessageReaderException)
    {
      _messengerReceiveService.NackMessage(message.MessageId);
    }
    catch (Exception ex)
    {
      _messengerReceiveService.NackMessage(message.MessageId);
      _logger.LogError(ex, "{Topic}: {Message} - {Content}", message.Topic, ex.Message, message.Content);
    }
  }

  private void RunIntegrationEventHandler(Message message, Type integrationEventType, IIntegrationEventHandler handler)
  {
    try
    {
      var @event = JsonSerializer.Deserialize(message.Content, integrationEventType, _jsonSerializerOptions) as IntegrationEvent ??
                      throw new JsonException("The message could not be deserialized.");

      handler.Handle(@event).Wait();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "{Topic}: {Message} - {Content}", message.Topic, ex.Message, message.Content);
      throw new MessageReaderException(ex.Message, ex);
    }
  }

  protected virtual List<string> GetQueues(IServiceProvider serviceProvider)
  {
    return QueueNames;
  }

  protected override Task ExecuteAsync(CancellationToken stoppingToken)
  {
    if (QueueNames.Count != 0)
    {
      _ = Task.Run(async () =>
      {
        var retryPolicy = Policy
        .Handle<Exception>()
        .WaitAndRetryForeverAsync(
          sleepDurationProvider: retryAttempt =>
            TimeSpan.FromSeconds(5),
          onRetry: (exception, retryCount, timeSpan) =>
            _logger.LogError($"Retry. Error: {exception.Message}")
          );

        await retryPolicy.ExecuteAsync(async () =>
        {
          try
          {
            using var scope = _serviceScopeFactory.CreateScope();

            _messengerReceiveService = scope.ServiceProvider.GetRequiredService<IMessengerReceiveService>();
            _messengerReceiveService.OnMessageReceived += OnMessageReceivedEvent;
            var queues = GetQueues(scope.ServiceProvider);
            _messengerReceiveService.SubscribeQueues(queues);

            await Task.Delay(Timeout.Infinite, stoppingToken);
          }
          catch (TaskCanceledException ex)
          {
            _logger.LogInformation(ex, "The task was cancelled");
            _messengerReceiveService.UnsubscribeQueues();
            throw;
          }
          catch (Exception ex)
          {
            _logger.LogError(ex, "Error in reader {Name} - {Message}", this.GetType().Name, ex.Message);
            _messengerReceiveService?.UnsubscribeQueues();
            throw;
          }
        });
      }, stoppingToken);
    }

    return Task.CompletedTask;
  }
}