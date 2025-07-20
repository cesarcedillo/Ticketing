using Serilog.Core;
using Serilog.Events;
using System.Diagnostics;

namespace Ticketing.Core.Observability.Serilog;

/// <summary>
/// Adds the current Activity's TraceId and SpanId (OpenTelemetry compatible) to all logs.
/// </summary>
public class OpenTelemetryTraceEnricher : ILogEventEnricher
{
  public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
  {
    var activity = Activity.Current;
    if (activity != null)
    {
      logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("TraceId", activity.TraceId.ToString()));
      logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("SpanId", activity.SpanId.ToString()));
      if (!string.IsNullOrEmpty(activity.TraceStateString))
      {
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("TraceState", activity.TraceStateString));
      }
    }
  }
}
