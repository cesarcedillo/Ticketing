namespace Ticketing.Core.Service.Messenger.Interfaces;
public interface IMessengerSendService
{
  void BeginTransaction();
  void SendMessage(string body, string topic);
  void Commit();
  void RollBack();


  Task BeginTransactionAsync(CancellationToken cancellationToken = default);
  Task SendMessageAsync(string body, string topic, CancellationToken cancellationToken = default);
  Task CommitAsync(CancellationToken cancellationToken = default);
  Task RollBackAsync(CancellationToken cancellationToken = default);
}
