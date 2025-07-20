namespace Ticketing.Core.OpenTelemetry.Helpers;

public class OpenTelemetryOptionsBuilder
{
  private string _serviceName = "Unknown-Service";
  private float _samplingRatio = 1.0F;
  private List<string> _excludedPaths = [];
  private List<string> _propertiesToTrace = [];
  private bool _traceContents = false;

  public OpenTelemetryOptionsBuilder SetCloudRoleName(string cloudRoleName)
  {
    ArgumentException.ThrowIfNullOrEmpty(cloudRoleName);
    _serviceName= cloudRoleName;
    return this;
  }

  public OpenTelemetryOptionsBuilder SetConnectionString(string connectionString)
  {
    ArgumentException.ThrowIfNullOrEmpty(connectionString);
    _serviceName= connectionString;
    return this;
  }

  public OpenTelemetryOptionsBuilder SetSamplingRatio(float samplingRatio)
  {
    ArgumentOutOfRangeException.ThrowIfGreaterThan(samplingRatio, 1.0F);
    ArgumentOutOfRangeException.ThrowIfLessThan(samplingRatio, 0.0F);
    _samplingRatio = samplingRatio;
    return this;
  }

  public OpenTelemetryOptionsBuilder SetExcludedPaths(List<string> excludedPaths)
  {
    _excludedPaths = excludedPaths;
    return this;
  }

  public OpenTelemetryOptionsBuilder SetPropertiesToTrace(List<string> propertiesToTrace)
  {
    _propertiesToTrace = propertiesToTrace;
    return this;
  }

  public OpenTelemetryOptionsBuilder SetTraceContents(bool traceContents)
  {
    _traceContents = traceContents;
    return this;
  }

  public OpenTelemetryOptions Build()
  {
    return new OpenTelemetryOptions()
    {
      ServiceName = _serviceName,
      SamplingRatio = _samplingRatio,
      ExcludedPaths = _excludedPaths,
      PropertiesToTrace = _propertiesToTrace,
      TraceContents = _traceContents
    };
  }
}
