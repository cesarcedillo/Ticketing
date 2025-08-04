using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using Ticketing.Core.Service.Messenger.Interfaces;
using Ticketing.Core.Service.Messenger.RabbitMQ.Interfaces;
using Ticketing.Core.Service.Messenger.RabbitMQ.Services;
using Ticketing.Core.Service.Messenger.Types;

namespace Ticketing.Core.Service.Messenger.RabbitMQ.Extensions;
public static class MessengerServiceRabbitExtension
{
  public static IServiceCollection AddRabbitMQConfiguration(this IServiceCollection services,
      Uri rabbitMQConnection,
      string connectionName,
      BrokersConfiguration brokersConfiguration,
      MessagingConfiguration messagingConfiguration)
  {
    brokersConfiguration.ValidateConfiguration();

    return services
        .AddSingleton(CreateRabbitMQConnection(rabbitMQConnection, connectionName, brokersConfiguration, messagingConfiguration))
        .AddSingleton<IChannelPool, ChannelPool>()
        .AddScoped<IRabbitMQChannel, RabbitMQChannel>()
        .AddTransient<IMessengerSendService, MessengerSendServiceRabbit>()
        .AddTransient<IMessengerReceiveService, MessengerReceiveServiceRabbit>();
  }

  private static Func<IServiceProvider, IConnection> CreateRabbitMQConnection(Uri rabbitMQConnection, string connectionName, BrokersConfiguration brokersConfiguration, MessagingConfiguration messagingConfiguration)
  {
    return services =>
    {
      ConnectionFactory factory = new()
      {
        Uri = rabbitMQConnection,
        AutomaticRecoveryEnabled = true,
        ClientProvidedName = connectionName
      };
      IConnection connection = factory.CreateConnection();
      IModel model = connection.CreateModel();

      model.CreateExchanges(brokersConfiguration);
      model.CreateQueues(brokersConfiguration, messagingConfiguration);
      model.BindQueues(brokersConfiguration, messagingConfiguration);
      model.Close();
      model.Dispose();
      return connection;
    };
  }
}
