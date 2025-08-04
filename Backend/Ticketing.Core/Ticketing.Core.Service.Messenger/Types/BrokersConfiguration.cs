namespace Ticketing.Core.Service.Messenger.Types;
public class BrokersConfiguration : List<BrokerConfiguration>
{
  public BrokerConfiguration ErrorsBroker
  {
    get { return this.First(broker => broker.ErrorBroker); }
  }
  public List<BrokerConfiguration> Brokers
  {
    get { return this.FindAll(broker => !broker.ErrorBroker); }
  }

  public List<BrokerConfiguration> AllBrokers => this;
  public void ValidateConfiguration()
  {
    CheckDuplicateTopicsInExchanges();
    CheckErrorsBroker();
  }
  private void CheckDuplicateTopicsInExchanges()
  {
    ForEach(broker =>
    {
      if (Exists(otherBroker =>
              !otherBroker.BrokerName.Equals(broker.BrokerName) &&
              otherBroker.Topics.Intersect(broker.Topics).Any()))
      {
        throw new InvalidDataException("There is a topic in more than one broker");
      }
    });
  }
  private void CheckErrorsBroker()
  {
    var results = FindAll(broker => broker.ErrorBroker);
    if (results.Count == 0)
      throw new InvalidDataException("There is no broker error configured");
    else if (results.Count > 1)
      throw new InvalidDataException("There are more than 1 broker errors configured");
  }
}

public class BrokerConfiguration
{
  public BrokerConfiguration()
  {
    BrokerName = "empty-broker";
    Topics = new List<string>();
    ErrorBroker = false;
  }
  public BrokerConfiguration(string brokerName, List<string> topics, bool errorBroker)
  {
    ArgumentNullException.ThrowIfNull(topics);
    if (topics.Count == 0)
    {
      throw new InvalidDataException($"There are no topics in the broker {brokerName}");
    }

    BrokerName = string.IsNullOrEmpty(brokerName) ? throw new ArgumentNullException(nameof(brokerName)) : brokerName;
    Topics = topics;
    ErrorBroker = errorBroker;
  }

  public string BrokerName { get; set; }
  public List<string> Topics { get; set; }
  public bool ErrorBroker { get; set; }
}