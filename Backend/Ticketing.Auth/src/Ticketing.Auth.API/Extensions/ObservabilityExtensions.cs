using Ticketing.Core.Observability.OpenTelemetry;
using Ticketing.Core.Observability.OpenTelemetry.Options;
using Ticketing.Core.Observability.Serilog;

namespace Ticketing.Auth.API.Extensions
{
  public static class ObservabilityExtensions
  {
    public static void AddObservability(this WebApplicationBuilder builder, IConfiguration configuration)
    {
      builder.Host.UseSerilogWithOptions(new SerilogOptions
      {
        MinimumLevel = configuration.GetValue<string>("MinimumLevel", "Information"),
        Sink = configuration.GetValue<string>("Sink", "Console"),
        FilePath = configuration["Sink"] == "File" ?
                                  configuration.GetValue<string>("FilePath", "Logs/log-.txt") :
                                  ""
      });

      builder.Services.AddOpenTelemetryCustom(new ZipkinOptions
      {
        ServiceName = configuration.GetValue<string>("ServiceName", "Unknown-Service"),
        PropertiesToTrace = configuration.GetSection("PropertiesToTrace").Get<List<string>>() ?? [],
        SamplingRatio = configuration.GetValue<float>("SamplingRatio", 1.0F),
        TraceContents = configuration.GetValue<bool>("TraceContents", false),
        ExcludedPaths = configuration.GetSection("ExcludedPaths").Get<List<string>>() ?? [],
        AgentHost = configuration.GetValue<string>("AgentHost", "zipkin"),
        AgentPort = configuration.GetValue<int>("AgentPort", 9411),
      });

      Console.WriteLine(">> OpenTelemetry configurado. Deberías ver spans en Jaeger en breves.");

      Console.WriteLine($"ServiceName: {configuration.GetValue<string>("ServiceName")}");
      Console.WriteLine($"AgentHost: {configuration.GetValue<string>("AgentHost")}");
      Console.WriteLine($"AgentPort: {configuration.GetValue<string>("AgentPort")}");

    }
  }
}
