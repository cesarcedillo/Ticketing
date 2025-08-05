using Ticketing.Notifications.API.HealthChecks;

namespace Ticketing.Notifications.API.BackgroundServices.HealthChecks;
public class StartupBackgroundService : BackgroundService
{
  private readonly StartupHealthCheck _healthCheck;

  public StartupBackgroundService(StartupHealthCheck healthCheck)
      => _healthCheck = healthCheck;

  protected override Task ExecuteAsync(CancellationToken stoppingToken)
  {
    _healthCheck.StartupCompleted = true;
    return Task.CompletedTask;
  }
}