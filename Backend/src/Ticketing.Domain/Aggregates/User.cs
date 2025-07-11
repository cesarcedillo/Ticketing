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

    public User(Guid userId, string userName, byte[] avatar, UserType userType)
    {
      Id = userId;
      UserName = userName;
      Avatar = avatar;
      UserType = userType;
    }
  }
}
