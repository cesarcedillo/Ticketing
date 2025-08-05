using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ticketing.Core.Service.Messenger.Implementations;
using Ticketing.Core.Service.Messenger.Interfaces;
using Ticketing.Core.Service.Messenger.RabbitMQ.Extensions;
using Ticketing.Core.Service.Messenger.Types;

namespace Ticketing.Ticket.Application.Extensions;
public static class MessengerConfigurationExtension
{
  public static IServiceCollection AddMessengerConfiguration(this IServiceCollection services, IConfiguration configuration)
  {
    var messagingConfiguration = configuration.GetRequiredSection("MessagingConfiguration");
    var brokerConfigurations = configuration.GetRequiredSection("BrokerConfigurations");
    var rabbitMQConnection = configuration["RabbitMQConnection"];

    if ((messagingConfiguration is null) || (brokerConfigurations is null) || string.IsNullOrWhiteSpace(rabbitMQConnection))
    {
      throw new Exception("Error collecting messaging settings: " +
          $"{nameof(messagingConfiguration)} exists -> [{messagingConfiguration is not null}]," +
          $"{nameof(brokerConfigurations)} exists -> [{brokerConfigurations is not null}]," +
          $"{nameof(brokerConfigurations)} exists -> [{string.IsNullOrWhiteSpace(rabbitMQConnection)}]");
    }

    services.Configure<MessagingConfiguration>(messagingConfiguration);
    services.Configure<BrokersConfiguration>(brokerConfigurations);

    services.AddSingleton<IMessengerBrokerDiscover, MessengerBrokerDiscover>();
    services.AddRabbitMQConfiguration
        (new Uri(rabbitMQConnection),
        configuration["ServiceName"] ?? throw new Exception("The service name could not be found."),
        brokerConfigurations.Get<BrokersConfiguration>()!,
        messagingConfiguration.Get<MessagingConfiguration>()!);

    return services;
  }
}