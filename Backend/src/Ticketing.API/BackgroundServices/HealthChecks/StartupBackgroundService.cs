﻿using Ticketing.API.HealthChecks;

namespace Ticketing.API.BackgroundServices.HealthChecks;


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

