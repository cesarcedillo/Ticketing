namespace Ticketing.Core.Service.Messenger.Interfaces;
public interface IMessengerSendService
{
  void BeginTransaction();
  void SendMessage(string body, string topic);
  void Commit();
  void RollBack();
}
