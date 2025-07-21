using Ticketing.Auth.Domain.Aggregates;
using Ticketing.Auth.Domain.Enum;
using Ticketing.Auth.TestCommon.BuildersM;

namespace Ticketing.Auth.TestCommon.Fixtures;
public class DomainFixture
{
  public User CreateDefaultCustomer(string? username = null)
  {
    return new UserBuilder()
        .WithUsername(username ?? "customer")
        .WithRole(Role.Customer)
        .Build();
  }
  public User CreateDefaultAdmin(string? username = null)
  {
    return new UserBuilder()
        .WithUsername(username ?? "admin")
        .WithRole(Role.Admin)
        .Build();
  }
  public User CreateDefaultAgent(string? username = null)
  {
    return new UserBuilder()
        .WithUsername(username ?? "agent")
        .WithRole(Role.Agent)
        .Build();
  }
}
