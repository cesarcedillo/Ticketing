using Ticketing.Auth.Domain.Aggregates;
using Ticketing.Auth.Domain.Enums;
using Ticketing.Auth.Infrastructure.Services;

namespace Ticketing.Auth.TestCommon.BuildersM;

  public class UserBuilder
{

  private SimplePasswordHasher passwordHasher = new SimplePasswordHasher();

  private string _username = "username";
  private string _password = "1234";
  private Role _role = Role.Customer;

  public UserBuilder WithUsername(string username)
  {
    _username = username;
    return this;
  }

  public UserBuilder WithPassword(string password)
  {
    _password = password;
    return this;
  }

  public UserBuilder WithRole(Role role)
  {
    _role = role;
    return this;
  }

  public User Build()
  {
    return new User(_username, passwordHasher.Hash(_password), _role);
  }
}
