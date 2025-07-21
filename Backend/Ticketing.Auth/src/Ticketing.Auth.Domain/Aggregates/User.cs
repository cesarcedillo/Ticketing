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
  public User(string userName, string passwordHash, Role role)
  {
    if (string.IsNullOrWhiteSpace(userName))
      throw new ArgumentException("Username cannot be empty.", nameof(userName));

    if (string.IsNullOrWhiteSpace(passwordHash))
      throw new ArgumentException("Password cannot be empty.", nameof(passwordHash));

    if (!System.Enum.IsDefined(typeof(Role), role))
      throw new ArgumentException("Invalid Role.", nameof(role));

    Id = Guid.NewGuid();
    UserName = userName;
    PasswordHash = passwordHash;
    Role = role;
  }
}
