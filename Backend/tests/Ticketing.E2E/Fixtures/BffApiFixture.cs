using System.Net.Http.Headers;
using System.Net.Http.Json;
using Ticketing.E2E.Helpers;
using Ticketing.E2E.TestData;

namespace Ticketing.E2E.Fixtures;

public sealed class BffApiFixture : IAsyncLifetime
{
  private readonly TestSettings _settings = TestSettings.Load();

  public HttpClient Client { get; private set; } = null!;
  public string? AccessToken { get; private set; }
  public UserResponseBff? CurrentUser { get; private set; }

  public async Task InitializeAsync()
  {
    Client = new HttpClient { BaseAddress = _settings.BffBaseUrl, Timeout = _settings.HttpTimeout };
    await SignInAsync();
  }

  public Task DisposeAsync()
  {
    Client.Dispose();
    return Task.CompletedTask;
  }

  public async Task SignInAsync()
  {
    var payload = new { username = _settings.Username, password = _settings.Password };
    var response = await Client.PostAsJsonAsync("/api/User/signin", payload);

    // para depurar rápido:
    var content = await response.Content.ReadAsStringAsync();
    response.EnsureSuccessStatusCode();

    var login = System.Text.Json.JsonSerializer.Deserialize<LoginResponseBff>(
      content,
      new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    if (login is null || !login.Success || string.IsNullOrWhiteSpace(login.AccessToken))
      throw new InvalidOperationException("SignIn failed or token missing.");

    AccessToken = login.AccessToken;
    CurrentUser = login.User;

    Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
  }
}
