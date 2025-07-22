using Ticketing.Core.Domain.SeedWork.Interfaces;
using Ticketing.User.Domain.Enums;

namespace Ticketing.User.Domain.Aggregates;
public class User : IAggregateRoot
{
  public Guid Id { get; private set; }
  public string UserName { get; private set; } = string.Empty;
  public string Avatar { get; private set; } = string.Empty;
  public Role UserType { get; private set; } = Role.Customer;

  public User() { }

  public User(string userName, string avatar, Role userType)
  {

    if (string.IsNullOrWhiteSpace(userName))
      throw new ArgumentException("Username cannot be empty.", nameof(userName));

    if (string.IsNullOrWhiteSpace(avatar))
      throw new ArgumentException("Avatar cannot be empty.", nameof(avatar));

    if (!Enum.IsDefined(typeof(Role), userType))
      throw new ArgumentException("Invalid UserType.", nameof(userType));

    Id = Guid.NewGuid();
    UserName = userName;
    Avatar = avatar;
    UserType = userType;
  }
}