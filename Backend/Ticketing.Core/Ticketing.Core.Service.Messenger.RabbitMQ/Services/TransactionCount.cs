namespace Ticketing.Core.Service.Messenger.RabbitMQ.Services;
public class TransactionCount
{
  private int count = 0;
  public void Increase()
  {
    count++;
  }
  public void Decrease()
  {
    if (count > 0)
    {
      count--;
    }
    else
    {
      count = 0;
    }
  }
  public void Reset()
  {
    count = 0;
  }

  public bool IsTransactionActive => count > 0;
}