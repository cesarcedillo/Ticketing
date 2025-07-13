using Ticketing.Domain.Aggregates;
using Ticketing.Domain.Enums;

namespace Ticketing.TestCommon.Builders;
public class UserBuilder
{
  private string _username = "username";
  private byte[] _avatar = [1, 2, 3, 4, 5];
  private UserType _userType = UserType.Customer;

  public UserBuilder WithUsername(string username)
  {
    _username = username;
    return this;
  }

  public UserBuilder WithAvatar(byte[] avatar)
  {
    _avatar = avatar;
    return this;
  }

  public UserBuilder WithUserType(UserType userType)
  {
    _userType = userType;
    return this;
  }

  public User Build()
  {
    return new User(_username, _avatar, _userType);
  }
}
