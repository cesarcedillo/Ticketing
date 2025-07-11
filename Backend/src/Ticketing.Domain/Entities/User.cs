using Ticketing.Domain.Aggregates;
using Ticketing.Domain.Enums;

namespace Ticketing.Domain.Entities
{
  public class User
  {
    public Guid Id { get; private set; }
    public string UserName { get; private set; } = string.Empty;
    public byte[] Avatar { get; private set; } = [];
    public UserType UserType { get; private set; } = UserType.Customer;

    private User() { }

    public User(Guid userId, string userName, byte[] avatar, UserType userType)
    {
      Id = userId;
      UserName = userName;
      Avatar = avatar;
      UserType = userType;
    }
  }
}
