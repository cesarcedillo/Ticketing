using FluentAssertions;
using Ticketing.Auth.Infrastructure.Services;

namespace Ticketing.Auth.Infrastructure.Tests.Services;
public class SimplePasswordHasherTests
{
  private readonly SimplePasswordHasher _hasher = new();

  [Fact]
  public void Hash_ShouldReturnSameValue_ForSamePassword()
  {
    var password = "TestPassword123!";
    var hash1 = _hasher.Hash(password);
    var hash2 = _hasher.Hash(password);

    hash1.Should().Be(hash2);
  }

  [Fact]
  public void Hash_ShouldReturnDifferentValue_ForDifferentPasswords()
  {
    var hash1 = _hasher.Hash("Password1");
    var hash2 = _hasher.Hash("Password2");

    hash1.Should().NotBe(hash2);
  }

  [Fact]
  public void Verify_ShouldReturnTrue_ForCorrectPassword()
  {
    var password = "MySecret";
    var hash = _hasher.Hash(password);

    var result = _hasher.Verify(password, hash);

    result.Should().BeTrue();
  }

  [Fact]
  public void Verify_ShouldReturnFalse_ForIncorrectPassword()
  {
    var hash = _hasher.Hash("RightPassword");

    var result = _hasher.Verify("WrongPassword", hash);

    result.Should().BeFalse();
  }

  [Theory]
  [InlineData("")]
  [InlineData(" ")]
  [InlineData(null)]
  public void Hash_ShouldReturnSameInput_ForNullOrWhitespace(string password)
  {
    var hash = _hasher.Hash(password);

    hash.Should().Be(password);
  }

  [Fact]
  public void Verify_ShouldReturnFalse_ForNullPasswordHash()
  {
    var result = _hasher.Verify("somepassword", null);

    result.Should().BeFalse();
  }
}

