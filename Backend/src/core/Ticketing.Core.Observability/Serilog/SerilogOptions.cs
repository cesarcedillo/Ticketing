namespace Ticketing.Core.Observability.Serilog;

public class SerilogOptions
{
  /// <summary>
  /// Logging minimum level (e.g., Information, Warning, Error)
  /// </summary>
  public string MinimumLevel { get; set; } = "Information";

  /// <summary>
  /// Output sink: "Console", "File", "Seq", "Elastic", etc.
  /// </summary>
  public string Sink { get; set; } = "Console";

  /// <summary>
  /// File path for file sink (optional)
  /// </summary>
  public string? FilePath { get; set; }

  /// <summary>
  /// Additional custom options, as needed
  /// </summary>
  public Dictionary<string, string>? Custom { get; set; }
}
