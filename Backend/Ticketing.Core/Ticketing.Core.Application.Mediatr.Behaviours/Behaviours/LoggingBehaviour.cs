using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace Ticketing.Core.Application.Mediatr.Behaviours.Behaviours;
public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
  private readonly ILogger<TRequest> _logger;

  public LoggingBehaviour(ILogger<TRequest> logger)
  {
    _logger = logger;
  }

  public Task Process(TRequest request, CancellationToken cancellationToken)
  {
    _logger.LogInformation("Manejando peticion del tipo {type} ", typeof(TRequest).Name);

    return Task.CompletedTask;
  }
}
