using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Ticketing.Ticket.API.HealthChecks;

public class StartupHealthCheck : IHealthCheck
{
  private volatile bool _isReady;

  public bool StartupCompleted
  {
    get => _isReady;
    set => _isReady = value;
  }

  public Task<HealthCheckResult> CheckHealthAsync(
      HealthCheckContext context, CancellationToken cancellationToken = default)
  {
    return Task.FromResult(StartupCompleted ? HealthCheckResult.Healthy("The Webapp was started successfully.") : HealthCheckResult.Unhealthy("The Webapp is starting"));
  }
}
