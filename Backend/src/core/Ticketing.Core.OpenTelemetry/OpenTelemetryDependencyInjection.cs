using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Ticketing.Core.OpenTelemetry.Helpers;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Ticketing.Core.OpenTelemetry;
public static class OpenTelemetryDependencyInjection
{
  [ExcludeFromCodeCoverage]
  public static IServiceCollection AddOpenTelemetryCustom(this IServiceCollection services, OpenTelemetryOptions options)
  {
    var resourceAttributes = new Dictionary<string, object> {
        { "service.name", options.ServiceName },
        { "telemetry.sdk.language", "dotnet" }
    };
    var excludedPaths = new List<string> { "health", "swagger" };
    excludedPaths.AddRange(options.ExcludedPaths);

    services
      .AddOpenTelemetry()
      .WithTracing(tracerBuilder => tracerBuilder
        .AddSource(options.ServiceName)
        .SetSampler(new AlwaysOnSampler())
        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddAttributes(resourceAttributes))
        .AddHttpClientInstrumentation(options =>
        {
          options.FilterHttpRequestMessage = (request) =>
          {
            if (request.RequestUri == null)
              return false;

            //if (request.RequestUri.Host.Contains("swagger", StringComparison.OrdinalIgnoreCase) ||
            //    request.RequestUri.Host.Contains("azconfig.io", StringComparison.OrdinalIgnoreCase) ||
            //    request.RequestUri.Host.Contains("appconfiguration.azure.com", StringComparison.OrdinalIgnoreCase))
            //  return false;

            return true;
          };
        })
        .AddAspNetCoreInstrumentation(opt =>
        {
          opt.Filter = (request) =>
              !(excludedPaths.Any(path => (request.Request.Path.Value?.Contains(path, StringComparison.OrdinalIgnoreCase) ?? false)));
        })
        .AddJaegerExporter(jaeger =>
        {
          jaeger.AgentHost = "localhost";
          jaeger.AgentPort = 6831;
        })
      )
      .ConfigureResource(resourceBuilder => resourceBuilder.AddAttributes(resourceAttributes));

    services.AddSingleton(new ActivitySource(options.ServiceName));
    services.Configure<OpenTelemetryOptions>(opts =>
    {
      opts.ServiceName = options.ServiceName;
      opts.SamplingRatio = options.SamplingRatio;
      opts.ExcludedPaths = options.ExcludedPaths;
      opts.PropertiesToTrace = options.PropertiesToTrace;
      opts.TraceContents = options.TraceContents;
    });

    return services;
  }

}