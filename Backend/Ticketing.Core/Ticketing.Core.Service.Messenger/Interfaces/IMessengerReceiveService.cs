using Ticketing.Core.Service.Messenger.Types;

namespace Ticketing.Core.Service.Messenger.Interfaces;
public interface IMessengerReceiveService
{
  void SubscribeQueues(List<string> queues);
  void UnsubscribeQueues();
  void AckMessage(string messageId);
  void NackMessage(string messageId);
  event EventHandler<Message> OnMessageReceived;
}
