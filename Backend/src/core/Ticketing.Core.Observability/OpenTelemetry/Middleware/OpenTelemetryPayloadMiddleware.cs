using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Text;

namespace Ticketing.Core.Observability.OpenTelemetry.Middleware;
public class OpenTelemetryPayloadMiddleware
{
  private readonly RequestDelegate _next;

  public OpenTelemetryPayloadMiddleware(RequestDelegate next)
  {
    _next = next;
  }

  public async Task Invoke(HttpContext context)
  {
    var activity = Activity.Current;

    if (context.Request.Method == "POST" || context.Request.Method == "PUT" || context.Request.Method == "PATCH")
    {
      context.Request.EnableBuffering();
      string body = "";
      using (var reader = new StreamReader(
          context.Request.Body,
          encoding: Encoding.UTF8,
          detectEncodingFromByteOrderMarks: false,
          bufferSize: 1024,
          leaveOpen: true))
      {
        body = await reader.ReadToEndAsync();
        context.Request.Body.Position = 0;
      }
      activity?.SetTag("http.request.body", body);
    }

    try
    {
      await _next(context);
    }
    catch (Exception ex)
    {
      if (activity != null)
      {
        activity.SetTag("otel.status_code", "ERROR");
        activity.SetTag("otel.status_description", ex.Message);
        activity.SetTag("exception.type", ex.GetType().Name);
        activity.SetTag("exception.message", ex.Message);
        activity.SetTag("exception.stacktrace", ex.StackTrace);
        activity?.SetTag("otel.error.message", ex.Message);
        activity?.SetTag("otel.error.stacktrace", ex.StackTrace);
      }
      throw;
    }
  }
}
