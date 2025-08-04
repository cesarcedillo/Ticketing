using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Ticketing.Core.Observability.OpenTelemetry.Options;

namespace Ticketing.Core.Observability.OpenTelemetry;
public static class OpenTelemetryDependencyInjection
{
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
    .WithTracing(tracerBuilder =>
      {
        tracerBuilder
        .AddSource(options.ServiceName)
        .SetSampler(new AlwaysOnSampler())
        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddAttributes(resourceAttributes))
        //.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(options.ServiceName))
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
          .AddAspNetCoreInstrumentation//();
        (opt =>
        {
          opt.Filter = (request) =>
              !(excludedPaths.Any(path => (request.Request.Path.Value?.Contains(path, StringComparison.OrdinalIgnoreCase) ?? false)));
        });

        switch (options)
        {
          case JaegerOptions jaeger:
            tracerBuilder.AddOtlpExporter(exporterOptions =>
            {
              exporterOptions.Endpoint = new Uri($"http://{jaeger.AgentHost}:{jaeger.AgentPort}");
            });

            break;

          case ZipkinOptions zipkin:
            tracerBuilder.AddZipkinExporter(options =>
              {
                options.Endpoint = new Uri($"http://{zipkin.AgentHost}:{zipkin.AgentPort}/api/v2/spans");
              });

            break;
          default:
            throw new NotSupportedException("Unknown OpenTelemetry exporter type");
        }
      })
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