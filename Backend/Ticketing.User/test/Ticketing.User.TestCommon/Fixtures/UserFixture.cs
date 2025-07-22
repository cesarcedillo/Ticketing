using Ticketing.User.Domain.Enums;
using Ticketing.User.TestCommon.Builders;
using UserType = Ticketing.User.Domain.Aggregates.User;

namespace Ticketing.User.TestCommon.Fixtures;
public class DomainFixture
{
  public UserType CreateDefaultCustomer(string? username = null)
  {
    return new UserBuilder()
        .WithUsername(username ?? "customer")
        .WithUserType(Role.Customer)
        .Build();
  }

  public UserType CreateDefaultAgent(string? username = null)
  {
    return new UserBuilder()
        .WithUsername(username ?? "agent")
    .WithUserType(Role.Agent)
        .Build();
  }
}

