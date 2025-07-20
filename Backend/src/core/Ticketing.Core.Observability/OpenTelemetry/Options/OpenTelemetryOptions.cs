namespace Ticketing.Core.Observability.OpenTelemetry.Options;

/// <summary>
/// Base options for OpenTelemetry tracing configuration.
/// </summary>
public abstract class OpenTelemetryOptions
{
  public string ServiceName { get; set; } = string.Empty;
  public float SamplingRatio { get; set; } = 1.0F;
  public List<string> ExcludedPaths { get; set; } = [];
  public List<string> PropertiesToTrace { get; set; } = [];
  public bool TraceContents { get; set; } = false;
}
