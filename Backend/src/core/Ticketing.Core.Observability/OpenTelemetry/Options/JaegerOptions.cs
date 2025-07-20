namespace Ticketing.Core.Observability.OpenTelemetry.Options;
public class JaegerOptions : OpenTelemetryOptions
{
  public string AgentHost { get; set; } = "localhost";
  public int AgentPort { get; set; } = 6831;
}