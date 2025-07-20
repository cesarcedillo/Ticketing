using Ticketing.Ticket.API.BackgroundServices.HealthChecks;
using Ticketing.Ticket.API.HealthChecks;

namespace Ticketing.Ticket.API.Extensions;

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

