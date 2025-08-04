using Microsoft.Extensions.Options;
using Ticketing.Core.Service.Messenger.Interfaces;
using Ticketing.Core.Service.Messenger.Types;

namespace Ticketing.Core.Service.Messenger.Implementations;
public class MessengerBrokerDiscover : IMessengerBrokerDiscover
{
  private readonly BrokersConfiguration Brokers;
  public MessengerBrokerDiscover(IOptions<BrokersConfiguration> brokers)
  {
    ArgumentNullException.ThrowIfNull(brokers);
    ArgumentNullException.ThrowIfNull(brokers.Value);

    Brokers = brokers.Value;
  }

  public string GetBrokerName(string topic)
  {
    BrokerConfiguration broker = Brokers.Find(broker => !broker.ErrorBroker && broker.Topics.Contains(topic)) ?? throw new InvalidDataException($"There's no broker for topic {topic}");

    return broker.BrokerName;
  }
}
