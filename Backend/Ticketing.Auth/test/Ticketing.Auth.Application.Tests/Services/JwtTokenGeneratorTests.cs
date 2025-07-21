using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Ticketing.Auth.Application.Dtos;
using Ticketing.Auth.Application.Services;
using Ticketing.Auth.Domain.Aggregates;
using Ticketing.Auth.Domain.Enums;

namespace Ticketing.Auth.Application.Tests.Services;
public class JwtTokenGeneratorTests
{
  private IConfiguration CreateConfiguration(Dictionary<string, string> settings)
  {
    return new ConfigurationBuilder()
        .AddInMemoryCollection(settings)
        .Build();
  }

  [Fact]
  public void GenerateToken_Should_Return_Valid_JwtTokenDto()
  {
    // Arrange
    var config = CreateConfiguration(new Dictionary<string, string>
    {
      ["Secret"] = "MY_SUPER_SECRET_1234567890_laksjdiasdfoiausdfiajsdñflkjasdñflkjasñdljfasñldkfjasñldfjañsldkjfañsldjfñasldjfñalsdjfñiejñirjweorijqwle",
      ["Issuer"] = "TestIssuer",
      ["Audience"] = "TestAudience",
      ["ExpiryHours"] = "1"
    });

    var generator = new JwtTokenGenerator(config);
    var user = new User("testuser", "hashed", Role.Agent);

    // Act
    var tokenDto = generator.GenerateToken(user);

    // Assert
    tokenDto.Should().NotBeNull();
    tokenDto.Token.Should().NotBeNullOrEmpty();
    tokenDto.Expiration.Should().BeAfter(DateTime.UtcNow);

    var handler = new JwtSecurityTokenHandler();
    var jwt = handler.ReadJwtToken(tokenDto.Token);

    jwt.Claims.Should().ContainSingle(c => c.Type == JwtRegisteredClaimNames.Sub && c.Value == "testuser");
    jwt.Claims.Should().ContainSingle(c => c.Type == JwtRegisteredClaimNames.Name && c.Value == "testuser");
    jwt.Claims.Should().ContainSingle(c => c.Type == System.Security.Claims.ClaimTypes.Name && c.Value == "testuser");
    jwt.Claims.Should().ContainSingle(c => c.Type == System.Security.Claims.ClaimTypes.Role && c.Value == "Agent");
    jwt.Issuer.Should().Be("TestIssuer");
    jwt.Audiences.Should().Contain("TestAudience");
  }

  [Fact]
  public void GenerateToken_Should_Use_Default_Expiry_When_Not_Configured()
  {
    // Arrange
    var config = CreateConfiguration(new Dictionary<string, string>
    {
      ["Secret"] = "MY_SUPER_SECRET_1234567890_laksjdiasdfoiausdfiajsdñflkjasdñflkjasñdljfasñldkfjasñldfjañsldkjfañsldjfñasldjfñalsdjfñiejñirjweorijqwle"
    });

    var generator = new JwtTokenGenerator(config);
    var user = new User("user", "hash", Role.Admin);

    // Act
    var tokenDto = generator.GenerateToken(user);

    // Assert
    tokenDto.Should().NotBeNull();
    tokenDto.Expiration.Should().BeAfter(DateTime.UtcNow);
  }
}
