using Ticketing.Auth.API.BackgroundServices.HealthChecks;
using Ticketing.Auth.API.HealthChecks;

namespace Ticketing.Auth.API.Extensions;

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

