using Ticketing.Auth.Domain.Enum;
using Ticketing.Core.Domain.SeedWork.Interfaces;

namespace Ticketing.Auth.Domain.Aggregates;
public class User : IAggregateRoot
{
  public Guid Id { get; private set; }
  public string UserName { get; private set; }
  public string PasswordHash { get; private set; }
  public Role Role { get; private set; }

  public User() { }
  public User(string username, string passwordHash, Role role)
  {
    Id = Guid.NewGuid();
    UserName = username;
    PasswordHash = passwordHash;
    Role = role;
  }
}
