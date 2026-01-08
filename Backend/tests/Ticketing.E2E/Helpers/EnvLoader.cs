using DotNetEnv;

namespace Ticketing.E2E.Helpers;

public static class EnvLoader
{
  private static bool _loaded;

  public static void LoadOnce()
  {
    if (_loaded) return;

    var overridePath = Environment.GetEnvironmentVariable("E2E_ENV_FILE");
    if (!string.IsNullOrWhiteSpace(overridePath) && File.Exists(overridePath))
    {
      Env.Load(overridePath);
      _loaded = true;
      return;
    }
    
    var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
    while (dir is not null && !File.Exists(Path.Combine(dir.FullName, "docker-compose.yml")))
      dir = dir.Parent;

    if (dir is null)
      throw new InvalidOperationException("Cannot locate repo root (docker-compose.yml not found). Set E2E_ENV_FILE env var to the e2e.env path.");

    var envPath = Path.Combine(dir.FullName, "Configuration", "e2e.env");
    if (!File.Exists(envPath))
      throw new InvalidOperationException($"E2E env file not found at: {envPath}");

    Env.Load(envPath);
    _loaded = true;
  }
}
