using RabbitMQ.Client;

namespace Ticketing.Core.Service.Messenger.RabbitMQ.Interfaces;
public interface IChannelPool : IDisposable
{
  IModel GetChannel();
  void ReleaseChannel(IModel channel);
}