using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace Ticketing.Core.Observability.Serilog;

public static class SerilogExtensions
{
  /// <summary>
  /// Configures Serilog with the specified options and any additional custom enrichers/sinks.
  /// Note: Enrichers like .WithActivityEnricher() should be added in the main API/Worker project.
  /// </summary>
  public static IHostBuilder UseSerilogWithOptions(
      this IHostBuilder hostBuilder,
      SerilogOptions options,
      Action<LoggerConfiguration>? customConfig = null)
  {
    return hostBuilder.UseSerilog((context, services, loggerConfig) =>
    {
      loggerConfig.MinimumLevel.Is(ParseLevel(options.MinimumLevel));

      // Add custom enrichers from Observability (optional)
      loggerConfig.Enrich.With(new OpenTelemetryTraceEnricher());
      loggerConfig.Enrich.With(new CustomEnricher());

      // Allow the consumer to add more enrichers (e.g., WithActivityEnricher)
      customConfig?.Invoke(loggerConfig);

      // Configure sink
      switch (options.Sink.ToLowerInvariant())
      {
        case "file":
          if (!string.IsNullOrWhiteSpace(options.FilePath))
            loggerConfig.WriteTo.File(options.FilePath);
          else
            loggerConfig.WriteTo.Console(); // fallback
          break;
        case "console":
        default:
          loggerConfig.WriteTo.Console();
          break;
          // Add more sinks as needed, e.g. Seq, Elastic, etc.
      }
    });
  }

  private static LogEventLevel ParseLevel(string level)
      => Enum.TryParse(level, true, out LogEventLevel lvl) ? lvl : LogEventLevel.Information;
}
