using Ticketing.Core.Domain.SeedWork.Interfaces;
using Ticketing.Domain.Enums;

namespace Ticketing.Domain.Aggregates
{
  public class User : IAggregateRoot
  {
    public Guid Id { get; private set; }
    public string UserName { get; private set; } = string.Empty;
    public byte[] Avatar { get; private set; } = [];
    public UserType UserType { get; private set; } = UserType.Customer;

    public User() { }

    public User(string userName, byte[] avatar, UserType userType)
    {
      
      if (string.IsNullOrWhiteSpace(userName))
        throw new ArgumentException("Username cannot be empty.", nameof(userName));

      if (avatar == null)
        throw new ArgumentNullException(nameof(avatar), "Avatar cannot be null.");
      if (avatar.Length == 0)
        throw new ArgumentException("Avatar cannot be empty.", nameof(avatar));

      if (!Enum.IsDefined(typeof(UserType), userType))
        throw new ArgumentException("Invalid UserType.", nameof(userType));

      Id = Guid.NewGuid();
      UserName = userName;
      Avatar = avatar;
      UserType = userType;
    }
  }
}
