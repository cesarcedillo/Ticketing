namespace Ticketing.Core.Service.Messenger.Interfaces;
public interface IMessengerBrokerDiscover
{
  string GetBrokerName(string topic);
}