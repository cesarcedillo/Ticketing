using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Diagnostics;
using System.Text;
using Ticketing.Core.Service.Messenger.RabbitMQ.Interfaces;

namespace Ticketing.Core.Service.Messenger.RabbitMQ.Services;

public sealed class RabbitMQChannel(IChannelPool channelPool, ILogger<RabbitMQChannel> logger) : IRabbitMQChannel, IAsyncDisposable
{
  private readonly IChannelPool _channelPool = channelPool;
  private readonly ILogger<RabbitMQChannel> _logger = logger;
  private readonly IModel _channel = channelPool.GetChannel();
  private readonly TransactionCount _transactionCount = new();

  public IModel Model => _channel;
  public bool IsOpen => _channel.IsOpen;
  public bool IsClosed => _channel.IsClosed;

  public void BeginTransaction()
  {
    EnsureChannelOpen("beginning");

    _transactionCount.Increase();
  }

  public void Commit()
  {
    EnsureChannelOpen("committing");

    _transactionCount.Decrease();

    if (!_transactionCount.IsTransactionActive)
    {
      _channel.TxCommit();
    }
  }

  public void RollBack()
  {
    _channel.TxRollback();
    _transactionCount.Reset();
  }

  public Task BeginTransactionAsync(CancellationToken cancellationToken)
      => Task.Run(BeginTransaction, cancellationToken);

  public Task CommitAsync(CancellationToken cancellationToken)
      => Task.Run(Commit, cancellationToken);

  public Task RollBackAsync(CancellationToken cancellationToken)
      => Task.Run(RollBack, cancellationToken);

  public Task PublishMessageAsync(string exchange, string topic, string body, CancellationToken cancellationToken = default)
      => Task.Run(() =>
      {
        PublishMessage(exchange, topic, body);
      }, cancellationToken);

  public void PublishMessage(string exchange, string topic, string body)
  {
    IBasicProperties props = _channel.CreateBasicProperties();
    props.ContentType = "application/json";
    props.DeliveryMode = 2;
    props.CorrelationId = Activity.Current?.Id ?? string.Empty;

    _channel.BasicPublish(
        exchange: exchange,
        routingKey: topic,
        body: Encoding.UTF8.GetBytes(body),
        basicProperties: props);
  }

  public void Nack(string messageId)
    => Nack(ulong.Parse(messageId));

  public void Nack(ulong messageId)
  {
    _channel.BasicNack(messageId, false, true);
  }

  public void Ack(string messageId)
    => Ack(ulong.Parse(messageId));

  public void Ack(ulong messageId)
  {
    _channel.BasicAck(messageId, false);
  }

  public string BasicConsume(string queue, IBasicConsumer consumer)
    => _channel.BasicConsume(queue, false, consumer);

  public void CancelConsume(string consumerId)
    => _channel.BasicCancelNoWait(consumerId);

  public void ClearTransaction()
  {
    if (_transactionCount.IsTransactionActive)
    {
      RollBack();
    }
  }

  public void Dispose()
  {
    try
    {
      ClearTransaction();
    }
    catch (Exception ex)
    {
      _logger?.LogWarning(ex, "Error rolling back ClearTransaction during Dispose.");
    }

    _channelPool.ReleaseChannel(_channel);
    GC.SuppressFinalize(this);
  }

  public ValueTask DisposeAsync()
  {
    Dispose();
    return ValueTask.CompletedTask;
  }

  private void EnsureChannelOpen(string operation)
  {
    if (_channel.IsClosed)
    {
      throw new ConnectFailureException($"Exception {operation} a Rabbit transaction: the channel is closed", null);
    }
  }
}
