using Ticketing.E2E.Helpers;

namespace Ticketing.E2E.TestData;

public sealed class TestSettings
{
  public required Uri BffBaseUrl { get; init; }
  public required string Username { get; init; }
  public required string Password { get; init; }
  public required TimeSpan HttpTimeout { get; init; }

  public static TestSettings Load()
  {
    EnvLoader.LoadOnce();

    string? baseUrl = Environment.GetEnvironmentVariable("E2E_BFF_BASE_URL");
    string? username = Environment.GetEnvironmentVariable("E2E_USERNAME");
    string? password = Environment.GetEnvironmentVariable("E2E_PASSWORD");
    string? timeoutStr = Environment.GetEnvironmentVariable("E2E_HTTP_TIMEOUT_SECONDS");

    if (string.IsNullOrWhiteSpace(baseUrl))
      throw new InvalidOperationException("Missing E2E_BFF_BASE_URL in Configuration/e2e.env");
    if (string.IsNullOrWhiteSpace(username))
      throw new InvalidOperationException("Missing E2E_USERNAME in Configuration/e2e.env");
    if (string.IsNullOrWhiteSpace(password))
      throw new InvalidOperationException("Missing E2E_PASSWORD in Configuration/e2e.env");

    int timeoutSeconds = 30;
    if (!string.IsNullOrWhiteSpace(timeoutStr) && int.TryParse(timeoutStr, out var parsed))
      timeoutSeconds = parsed;

    return new TestSettings
    {
      BffBaseUrl = new Uri(baseUrl, UriKind.Absolute),
      Username = username,
      Password = password,
      HttpTimeout = TimeSpan.FromSeconds(timeoutSeconds)
    };
  }
}
