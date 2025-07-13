using FluentAssertions;
using Ticketing.Domain.Enums;
using Ticketing.TestCommon.Builders;

namespace Ticketing.Domain.Tests.Entities;

public class UserTests
{
  [Fact]
  public void Create_User_Assigns_Properties_Correctly()
  {
    var username = "user_test";
    var avatar = new byte[] { 1, 2, 3, 4 };
    var userType = UserType.Agent;

    var user = new UserBuilder()
        .WithUsername(username)
        .WithAvatar(avatar)
        .WithUserType(userType)
        .Build();

    user.UserName.Should().Be(username);
    user.Avatar.Should().Equal(avatar);
    user.UserType.Should().Be(userType);
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
  public void Cannot_Create_User_With_Empty_Avatar(string? _)
  {
    Action act1 = () => new UserBuilder().WithAvatar(Array.Empty<byte>()).Build();
    Action act2 = () => new UserBuilder().WithAvatar(null!).Build();

    act1.Should().Throw<ArgumentException>();
    act2.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void Cannot_Create_User_With_Invalid_UserType()
  {
    var userBuilder = new UserBuilder().WithUserType((UserType)99);

    Action act = () => userBuilder.Build();

    act.Should().Throw<ArgumentException>();
  }
}
