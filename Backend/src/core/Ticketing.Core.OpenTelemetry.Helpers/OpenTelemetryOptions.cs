namespace Ticketing.Core.OpenTelemetry.Helpers;
public class OpenTelemetryOptions
{
  /// <summary>
  /// Logical name of the service.
  /// </summary>
  public string ServiceName { get; set; } = null!;

  /// <summary>
  /// Sampling ratio for traces (0 = none, 1 = sample all traces).
  /// </summary>
  public float SamplingRatio { get; set; } = 1.0F;

  /// <summary>
  /// List of HTTP request paths to exclude from tracing (e.g., health checks, swagger, metrics).
  /// </summary>
  public List<string> ExcludedPaths { get; set; } = [];

  /// <summary>
  /// List of custom business properties to be included as tags or baggage in traces.
  /// </summary>
  public List<string> PropertiesToTrace { get; set; } = [];

  /// <summary>
  /// Indicates whether to include request/response contents in traces (for debugging or auditing).
  /// </summary>
  public bool TraceContents { get; set; } = false;
}
