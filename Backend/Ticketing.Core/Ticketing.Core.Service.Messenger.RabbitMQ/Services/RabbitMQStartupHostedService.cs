using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace Ticketing.Core.Service.Messenger.RabbitMQ.Services;
public class RabbitMQStartupHostedService : IHostedService
{
  private readonly IConnection _connection;

  public RabbitMQStartupHostedService(IConnection connection)
  {
    _connection = connection;
  }

  public Task StartAsync(CancellationToken cancellationToken)
  {
    _ = _connection.IsOpen;
    return Task.CompletedTask;
  }

  public Task StopAsync(CancellationToken cancellationToken)
  {
    return Task.CompletedTask;
  }
}
