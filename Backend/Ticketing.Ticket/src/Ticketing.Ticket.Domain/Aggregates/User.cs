using Ticketing.Core.Domain.SeedWork.Interfaces;
using Ticketing.Ticket.Domain.Enums;

namespace Ticketing.Ticket.Domain.Aggregates
{
  public class User : IAggregateRoot
  {
    public Guid Id { get; private set; }
    public string UserName { get; private set; } = string.Empty;
    public string Avatar { get; private set; } = string.Empty;
    public UserType UserType { get; private set; } = UserType.Customer;

    public User() { }

    public User(string userName, string avatar, UserType userType)
    {
      
      if (string.IsNullOrWhiteSpace(userName))
        throw new ArgumentException("Username cannot be empty.", nameof(userName));

      if (string.IsNullOrWhiteSpace(avatar))
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
