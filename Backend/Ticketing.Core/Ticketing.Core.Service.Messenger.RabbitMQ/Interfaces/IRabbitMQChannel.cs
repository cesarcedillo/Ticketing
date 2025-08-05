using RabbitMQ.Client;

namespace Ticketing.Core.Service.Messenger.RabbitMQ.Interfaces;
public interface IRabbitMQChannel : IDisposable
{
  void BeginTransaction();
  void Commit();
  void RollBack();

  Task BeginTransactionAsync(CancellationToken cancellationToken = default);
  Task CommitAsync(CancellationToken cancellationToken = default);
  Task RollBackAsync(CancellationToken cancellationToken = default);
  void PublishMessage(string exchange, string topic, string body);
  Task PublishMessageAsync(string exchange, string topic, string body, CancellationToken cancellationToken = default);
  void Nack(string messageId);
  void Ack(string messageId);
  string BasicConsume(string queue, IBasicConsumer consumer);
  void CancelConsume(string consumerId);
  void ClearTransaction();
  bool IsOpen { get; }
  bool IsClosed { get; }
  IModel Model { get; }
}