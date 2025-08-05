using Ticketing.Notifications.API.BackgroundServices.HealthChecks;
using Ticketing.Notifications.API.HealthChecks;

namespace Ticketing.Notifications.API.Extensions;
public static class HealthChecksExtensions
{
  public static void AddHealthChecks(this WebApplicationBuilder builder, IConfiguration configuration)
  {
    builder.Services.AddHostedService<StartupBackgroundService>();
    builder.Services.AddSingleton<StartupHealthCheck>();

    builder.Services.AddHealthChecks()
      .AddCheck<StartupHealthCheck>(
        "startup",
        tags: new[] { "ready" });

  }
}