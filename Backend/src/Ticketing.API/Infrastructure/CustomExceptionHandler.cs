﻿using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Ticketing.Core.Application.Mediatr.Behaviours.Exceptions;

namespace Ticketing.API.Infrastructure;

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
                { typeof(Exception), HandleGenericException }
            };
  }

  public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
  {
    var exceptionType = exception.GetType();

    if (_exceptionHandlers.ContainsKey(exceptionType))
    {
      await _exceptionHandlers[exceptionType].Invoke(httpContext, exception);
      return true;
    }

    return false;
  }

  private async Task HandleValidationException(HttpContext httpContext, Exception ex)
  {
    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

    await httpContext.Response.WriteAsJsonAsync(new ProblemDetails()
    {
      Status = StatusCodes.Status400BadRequest,
      Title = "Bad request",
      Detail = ex.Message
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
}

