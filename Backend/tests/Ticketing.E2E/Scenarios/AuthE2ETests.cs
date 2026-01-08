using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Ticketing.E2E.Fixtures;
using Ticketing.E2E.Helpers;

namespace Ticketing.E2E.Scenarios;

[Trait("Category", "E2E")]
public class AuthE2ETests : IClassFixture<BffApiFixture>
{
  private readonly BffApiFixture _fx;

  public AuthE2ETests(BffApiFixture fx) => _fx = fx;

  [Fact]
  public void SignIn_OK_ShouldProvideToken()
  {
    _fx.AccessToken.Should().NotBeNullOrWhiteSpace();
  }

  [Fact]
  public async Task Me_WithToken_ShouldReturnUser()
  {
    var response = await _fx.Client.PostAsync("/api/User/Me", content: null);

    response.StatusCode.Should().Be(HttpStatusCode.OK);

    var me = await response.Content.ReadFromJsonAsync<UserResponseBff>(
      new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    me.Should().NotBeNull();
    me!.UserName.Should().NotBeNullOrWhiteSpace();
  }
}
