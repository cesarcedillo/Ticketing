using Microsoft.AspNetCore.Builder;
namespace Ticketing.Core.Observability.OpenTelemetry.Middleware;
public static class OpenTelemetryPayloadMiddlewareExtensions
{
  public static IApplicationBuilder UseOpenTelemetryPayload(this IApplicationBuilder builder)
  {
    return builder.UseMiddleware<OpenTelemetryPayloadMiddleware>();
  }
}
