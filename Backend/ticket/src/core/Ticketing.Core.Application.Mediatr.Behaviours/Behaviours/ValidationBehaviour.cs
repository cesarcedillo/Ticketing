using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Ticketing.Core.Application.Mediatr.Behaviours.Exceptions;

namespace Ticketing.Core.Application.Mediatr.Behaviours.Behaviours;

  public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
     where TRequest : notnull
{
  private readonly IEnumerable<IValidator<TRequest>> _validators;
  private readonly ILogger<TRequest> _logger;

  public ValidationBehaviour(
      IEnumerable<IValidator<TRequest>> validators,
      ILogger<TRequest> logger)
  {
    _validators = validators;
    _logger = logger;
  }

  public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
  {
    if (_validators.Any())
    {
      var context = new ValidationContext<TRequest>(request);

      var validationResults = await Task.WhenAll(
          _validators.Select(v =>
              v.ValidateAsync(context, cancellationToken)));

      var failures = validationResults
          .Where(r => r.Errors.Any())
          .SelectMany(r => r.Errors)
          .ToList();

      if (failures.Any())
      {
        _logger.LogError("Error handling request of type {type}, Error: " + string.Join("; ", failures.Select(x => x.ErrorMessage)), typeof(TRequest).Name);
        throw new ApplicationValidationException(failures);
      }
    }
    return await next();
  }
}
