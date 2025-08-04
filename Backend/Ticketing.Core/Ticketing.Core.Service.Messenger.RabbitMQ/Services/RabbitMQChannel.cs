using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Core.Service.Messenger.RabbitMQ.Interfaces;

namespace Ticketing.Core.Service.Messenger.RabbitMQ.Services;
public sealed class RabbitMQChannel(IChannelPool channelPool) : IRabbitMQChannel
{
  private readonly IChannelPool channelPool = channelPool;
  private readonly IModel channel = channelPool.GetChannel();
  private readonly TransactionCount transactionCount = new();

  public IModel Model => channel;
  public bool IsOpen => channel.IsOpen;
  public bool IsClosed => channel.IsClosed;

  public void BeginTransaction()
  {
    if (channel.IsClosed)
    {
      throw new ConnectFailureException("Exception begining a Rabbit transaction: the channel is closed", null);
    }

    transactionCount.Increase();
  }
  public void Commit()
  {
    if (channel.IsClosed)
    {
      throw new ConnectFailureException("Exception commiting a Rabbit transaction: the channel is closed", null);
    }

    transactionCount.Decrease();
    if (!transactionCount.IsTransactionActive)
    {
      channel.TxCommit();
    }
  }

  public void RollBack()
  {
    channel.TxRollback();
    transactionCount.Reset();
  }

  public void PublishMessage(string exchange, string topic, string body)
  {
    IBasicProperties props = channel.CreateBasicProperties();
    props.ContentType = "application/json";
    props.DeliveryMode = 2;
    props.CorrelationId = Activity.Current?.Id ?? string.Empty;

    channel.BasicPublish(
              exchange: exchange,
              routingKey: topic,
              body: Encoding.UTF8.GetBytes(body),
              basicProperties: props);
  }

  public void Nack(string messageId) { Nack(ulong.Parse(messageId)); }
  public void Nack(ulong messageId)
  {
    channel.BasicNack(messageId, false, true);
  }

  public void Ack(string messageId) { Ack(ulong.Parse(messageId)); }
  public void Ack(ulong messageId)
  {
    channel.BasicAck(messageId, false);
  }

  public string BasicConsume(string queue, IBasicConsumer consumer) =>
      channel.BasicConsume(queue, false, consumer);

  public void CancelConsume(string consumerId) =>
      channel.BasicCancelNoWait(consumerId);

  public void ClearTransaction()
  {
    if (transactionCount.IsTransactionActive)
    {
      RollBack();
    }
  }
  public void Dispose()
  {
    ClearTransaction();
    channelPool.ReleaseChannel(channel);
    GC.SuppressFinalize(this);
  }
}