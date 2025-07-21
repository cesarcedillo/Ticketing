using FluentAssertions;
using Ticketing.Auth.Domain.Enums;
using Ticketing.Auth.TestCommon.BuildersM;

namespace Ticketing.Auth.Domain.Tests.Aggregates;

public class UserTests
{
  [Fact]
  public void Create_User_Assigns_Properties_Correctly()
  {
    var username = "user_test";
    var password = "password";
    var role = Role.Agent;

    var user = new UserBuilder()
        .WithUsername(username)
        .WithPassword(password)
        .WithRole(role)
        .Build();

    user.UserName.Should().Be(username);
    user.PasswordHash.Should().NotBeNull();
    user.Role.Should().Be(role);
  }

  [Theory]
  [InlineData(null)]
  [InlineData("")]
  public void Cannot_Create_User_With_Empty_Username(string username)
  {
    Action act = () => new UserBuilder().WithUsername(username!).Build();

    act.Should().Throw<ArgumentException>();
  }

  [Theory]
  [InlineData(null)]
  [InlineData("")]
  public void Cannot_Create_User_With_Empty_Password(string password)
  {
    Action act1 = () => new UserBuilder().WithPassword(password).Build();

    act1.Should().Throw<ArgumentException>();
  }

  [Fact]
  public void Cannot_Create_User_With_Invalid_Role()
  {
    var userBuilder = new UserBuilder().WithRole((Role)99);

    Action act = () => userBuilder.Build();

    act.Should().Throw<ArgumentException>();
  }
}

