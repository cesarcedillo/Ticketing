using Ticketing.User.Domain.Enums;
using UserType = Ticketing.User.Domain.Aggregates.User;

namespace Ticketing.User.TestCommon.Builders;

  public class UserBuilder
{
  private string _username = "username";
  private string _avatar = "avatar";
  private Role _role = Role.Customer;

  public UserBuilder WithUsername(string username)
  {
    _username = username;
    return this;
  }

  public UserBuilder WithAvatar(string avatar)
  {
    _avatar = avatar;
    return this;
  }

  public UserBuilder WithUserType(Role role)
  {
    this._role = role;
    return this;
  }

  public UserType Build()
  {
    return new UserType(_username, _avatar, _role);
  }
}
