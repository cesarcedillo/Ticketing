using Serilog.Core;
using Serilog.Events;

namespace Ticketing.API.Extensions.Loggers.Configurations;

public class EnvironmentVariableLoggingLevelSwitch : LoggingLevelSwitch
{
  public EnvironmentVariableLoggingLevelSwitch(string environmentVariable)
  {
    LogEventLevel level = LogEventLevel.Information;
    if (Enum.TryParse<LogEventLevel>(Environment.ExpandEnvironmentVariables(environmentVariable), true, out level))
    {
      MinimumLevel = level;
    }
  }
}
