using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Ticketing.Core.Observability.OpenTelemetry.Options;
using Ticketing.Core.Service.Messenger.Implementations;
using Ticketing.Core.Service.Messenger.Interfaces;
using Ticketing.Core.Service.Messenger.RabbitMQ.Extensions;
using Ticketing.Core.Service.Messenger.Types;

namespace Ticketing.Notifications.Application.Extensions;
public static class MessengerConfigurationExtension
{
  public static IServiceCollection AddMessengerConfiguration(this IServiceCollection services, IConfiguration configuration)
  {
    var brokerJson = Environment.GetEnvironmentVariable("BrokerConfigurations");
    var messagingJson = Environment.GetEnvironmentVariable("MessagingConfiguration");
    var rabbitMQConnection = Environment.GetEnvironmentVariable("RabbitMQConnection");
    var serviceName = Environment.GetEnvironmentVariable("ServiceName");

    if (string.IsNullOrWhiteSpace(brokerJson) || string.IsNullOrWhiteSpace(messagingJson) || string.IsNullOrWhiteSpace(rabbitMQConnection))
    {
      throw new Exception("Error collecting messaging settings: " +
          $"BrokerConfigurations exists -> [{!string.IsNullOrWhiteSpace(brokerJson)}], " +
          $"MessagingConfiguration exists -> [{!string.IsNullOrWhiteSpace(messagingJson)}], " +
          $"RabbitMQConnection exists -> [{!string.IsNullOrWhiteSpace(rabbitMQConnection)}]");
    }

    var options = new JsonSerializerOptions
    {
      PropertyNameCaseInsensitive = true
    };

    var brokerConfigurations = JsonSerializer.Deserialize<BrokersConfiguration>(brokerJson, options)!;
    var messagingConfiguration = JsonSerializer.Deserialize<MessagingConfiguration>(messagingJson, options)!;

    services.AddSingleton(brokerConfigurations);
    services.AddSingleton(messagingConfiguration);

    services.AddSingleton<IMessengerBrokerDiscover, MessengerBrokerDiscover>();

    var openTelemetryOptions = services
    .BuildServiceProvider()
    .GetRequiredService<IOptions<OpenTelemetryOptions>>()
    .Value;


    services.AddRabbitMQConfiguration(
        new Uri(rabbitMQConnection),
        serviceName ?? throw new Exception("The service name could not be found."),
        brokerConfigurations,
        messagingConfiguration,
        openTelemetryOptions);

    return services;
  }
}