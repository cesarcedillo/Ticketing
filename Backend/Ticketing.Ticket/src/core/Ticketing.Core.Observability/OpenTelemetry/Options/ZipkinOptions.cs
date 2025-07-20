namespace Ticketing.Core.Observability.OpenTelemetry.Options;
public class ZipkinOptions : OpenTelemetryOptions
{
  public string AgentHost { get; set; } = "localhost";
  public int AgentPort { get; set; } = 9411;
}
