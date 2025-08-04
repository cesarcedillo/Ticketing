using RabbitMQ.Client;
using Ticketing.Core.Service.Messenger.RabbitMQ.Interfaces;

namespace Ticketing.Core.Service.Messenger.RabbitMQ.Services;
public sealed class ChannelPool : IChannelPool
{
  public const int POOL_CHANNEL_LIMIT = 10;

  private readonly Stack<IModel> FreeChannels = new();
  private readonly IList<IModel> AssignedChannels = [];
  private readonly IConnection Connection;
  private readonly int ChannelLimit;

  private readonly object LockObject = new();

  public int ChannelCount => FreeChannels.Count + AssignedChannels.Count;
  public int AssignedChannelCount => AssignedChannels.Count;
  public int FreeChannelCount => FreeChannels.Count;

  public ChannelPool(IConnection connection, int channelLimit = POOL_CHANNEL_LIMIT)
  {
    Connection = connection;
    ChannelLimit = channelLimit;
  }

  public IModel GetChannel()
  {
    lock (LockObject)
    {
      IModel channel = FreeChannels.Count > 0 ? FreeChannels.Pop() : CreateModel();
      AssignedChannels.Add(channel);
      return channel;
    }
  }

  public void ReleaseChannel(IModel channel)
  {
    lock (LockObject)
    {
      AssignedChannels.Remove(channel);
      if (FreeChannels.Count >= ChannelLimit)
      {
        channel.Close();
        channel.Dispose();
      }
      else
      {
        FreeChannels.Push(channel);
      }
    }
  }

  private IModel CreateModel()
  {
    IModel model = Connection.CreateModel();
    model.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
    model.TxSelect();
    return model;
  }

  public void Dispose()
  {
    FreeChannels.ToList().ForEach(x =>
    {
      x.Close();
      x.Dispose();
    });
    AssignedChannels.ToList().ForEach(x =>
    {
      x.Close();
      x.Dispose();
    });

    FreeChannels.Clear();
    AssignedChannels.Clear();
    GC.SuppressFinalize(this);
  }
}