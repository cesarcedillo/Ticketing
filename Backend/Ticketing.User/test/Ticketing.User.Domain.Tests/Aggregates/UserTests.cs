using FluentAssertions;
using Ticketing.User.Domain.Enums;
using Ticketing.User.TestCommon.Builders;

namespace Ticketing.User.Domain.Tests.Aggregates;
public class UserTests
{
  [Fact]
  public void Create_User_Assigns_Properties_Correctly()
  {
    var username = "user_test";
    var avatar = "avatar";
    var role = Role.Agent;

    var user = new UserBuilder()
        .WithUsername(username)
        .WithAvatar(avatar)
        .WithUserType(role)
        .Build();

    user.UserName.Should().Be(username);
    user.Avatar.Should().Be(avatar);
    user.UserType.Should().Be(role);
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
  public void Cannot_Create_User_With_Empty_Avatar(string avatar)
  {
    Action act1 = () => new UserBuilder().WithAvatar(avatar).Build();

    act1.Should().Throw<ArgumentException>();
  }

  [Fact]
  public void Cannot_Create_User_With_Invalid_UserType()
  {
    var userBuilder = new UserBuilder().WithUserType((Role)99);

    Action act = () => userBuilder.Build();

    act.Should().Throw<ArgumentException>();
  }
}