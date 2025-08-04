using RabbitMQ.Client;

namespace Ticketing.Core.Service.Messenger.RabbitMQ.Interfaces;
public interface IRabbitMQChannel : IDisposable
{
  void BeginTransaction();
  void Commit();
  void RollBack();
  void PublishMessage(string exchange, string topic, string body);
  void Nack(string messageId);
  void Ack(string messageId);
  string BasicConsume(string queue, IBasicConsumer consumer);
  void CancelConsume(string consumerId);
  void ClearTransaction();
  bool IsOpen { get; }
  bool IsClosed { get; }
  IModel Model { get; }
}