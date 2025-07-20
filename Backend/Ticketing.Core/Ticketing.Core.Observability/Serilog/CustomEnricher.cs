using Serilog.Core;
using Serilog.Events;

namespace Ticketing.Core.Observability.Serilog;

/// <summary>
/// Example of a custom enricher to add more properties to every log entry.
/// </summary>
public class CustomEnricher : ILogEventEnricher
{
  public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
  {
    // Example: add a static property to every log
    logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("Environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown"));
    // Add more custom properties here if needed
  }
}
