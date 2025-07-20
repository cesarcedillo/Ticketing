using Ticketing.Ticket.API.HealthChecks;

namespace Ticketing.Ticket.API.BackgroundServices.HealthChecks;


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

