using Auth.Cliente.NswagAutoGen.HttpClientFactoryImplementation;
using User.Cliente.NswagAutoGen.HttpClientFactoryImplementation;
using Microsoft.AspNetCore.Diagnostics;
using Ticketing.Core.Application.Mediatr.Behaviours.Exceptions;
using AuthProblemDetails = Auth.Cliente.NswagAutoGen.HttpClientFactoryImplementation.ProblemDetails;
using UserProblemDetails = Auth.Cliente.NswagAutoGen.HttpClientFactoryImplementation.ProblemDetails;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace Ticketing.BFF.API.Infrastructure;
public class CustomExceptionHandler : IExceptionHandler
{
  private readonly Dictionary<Type, Func<HttpContext, Exception, Task>> _exceptionHandlers;

  public CustomExceptionHandler()
  {
    _exceptionHandlers = new()
            {
                { typeof(ApplicationValidationException), HandleValidationException },
                { typeof(KeyNotFoundException), HandleNotFoundException },
                { typeof(ArgumentNullException), HandleArgumentException },
                { typeof(ArgumentException), HandleArgumentException },
                { typeof(InvalidOperationException), HandleInvalidOperationException},
                { typeof(AuthApiException), HandleAuthApiException},
                { typeof(UserApiException), HandleUserApiException},
                { typeof(Exception), HandleGenericException }
            };
  }

  public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
  {
    var exceptionType = exception.GetType();

    if (_exceptionHandlers.TryGetValue(exceptionType, out var handler) ||
        _exceptionHandlers.TryGetValue(exceptionType.BaseType!, out handler))
    {
      await handler.Invoke(httpContext, exception);
      return true;
    }

    return false;
  }

  private async Task HandleValidationException(HttpContext httpContext, Exception ex)
  {
    var exception = (ApplicationValidationException)ex;

    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

    await httpContext.Response.WriteAsJsonAsync(new ProblemDetails()
    {
      Status = StatusCodes.Status400BadRequest,
      Title = "Validation error",
      Detail = ex.Message,
      Extensions = exception.Errors.ToDictionary(
            keySelector: e => e.Key,
            elementSelector: e => (object?)e.Value
        )
    });
  }

  private async Task HandleNotFoundException(HttpContext httpContext, Exception ex)
  {
    httpContext.Response.StatusCode = StatusCodes.Status404NotFound;

    await httpContext.Response.WriteAsJsonAsync(new ProblemDetails()
    {
      Status = StatusCodes.Status404NotFound,
      Title = "Not found",
      Detail = ex.Message
    });
  }

  private async Task HandleArgumentException(HttpContext httpContext, Exception ex)
  {
    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

    await httpContext.Response.WriteAsJsonAsync(new ProblemDetails()
    {
      Status = StatusCodes.Status400BadRequest,
      Title = "Bad request",
      Detail = ex.Message
    });
  }

  private async Task HandleInvalidOperationException(HttpContext httpContext, Exception ex)
  {
    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

    await httpContext.Response.WriteAsJsonAsync(new ProblemDetails()
    {
      Status = StatusCodes.Status400BadRequest,
      Title = "Invalid Operation Exception",
      Detail = ex.InnerException?.Message ?? ex.Message
    });
  }

  private async Task HandleGenericException(HttpContext httpContext, Exception ex)
  {
    httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

    await httpContext.Response.WriteAsJsonAsync(new ProblemDetails()
    {
      Status = StatusCodes.Status500InternalServerError,
      Title = "Internal server error - " + ex.Source,
      Detail = ex.Message
    });
  }

  private async Task HandleAuthApiException(HttpContext httpContext, Exception ex)
  {
    var exceptionDetailed = ex as AuthApiException<AuthProblemDetails>;
    var exceptionGeneral = ex as AuthApiException;

    var statusCode = exceptionDetailed?.Result?.Status
              ?? exceptionGeneral?.StatusCode
              ?? StatusCodes.Status500InternalServerError;

    var detail = exceptionDetailed?.Result?.Detail
               ?? exceptionGeneral?.Message
               ?? "Unhandled exception";

    httpContext.Response.StatusCode = statusCode;

    await httpContext.Response.WriteAsJsonAsync(new HttpValidationProblemDetails()
    {
      Status = statusCode,
      Title = "Client exception - " + ex.Source,
      Detail = detail
    });
  }

  private async Task HandleUserApiException(HttpContext httpContext, Exception ex)
  {
    var exceptionDetailed = ex as UserApiException<UserProblemDetails>;
    var exceptionGeneral = ex as UserApiException;

    var statusCode = exceptionDetailed?.Result?.Status
              ?? exceptionGeneral?.StatusCode
              ?? StatusCodes.Status500InternalServerError;

    var detail = exceptionDetailed?.Result?.Detail
               ?? exceptionGeneral?.Message
               ?? "Unhandled exception";

    httpContext.Response.StatusCode = statusCode;

    await httpContext.Response.WriteAsJsonAsync(new HttpValidationProblemDetails()
    {
      Status = statusCode,
      Title = "Client exception - " + ex.Source,
      Detail = detail
    });
  }
}
