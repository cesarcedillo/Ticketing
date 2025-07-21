using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Ticketing.Auth.Domain.Aggregates;
using Ticketing.Auth.Domain.Enum;
using Ticketing.Auth.Infrastructure.Data;
using Ticketing.Auth.Infrastructure.Data.Repositories;

namespace Ticketing.Auth.Infrastructure.Tests.Repositories;

public class UserRepositoryTests
{
  private readonly AuthDbContext _context;

  public UserRepositoryTests()
  {
    var contextOptions = new DbContextOptionsBuilder<AuthDbContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString())
        .Options;
    _context = new AuthDbContext(contextOptions);

    _context.Users.RemoveRange(_context.Users);
    _context.SaveChanges();

    var user1 = new User("alice", "1234", Role.Customer);
    var user2 = new User("bob", "1234", Role.Agent);

    _context.Users.AddRange(user1, user2);
    _context.SaveChanges();
  }

  [Fact]
  public async Task GetByUserNameAsync_Should_Return_User_When_Exists()
  {
    var repo = new UserRepository(_context);

    var user = await _context.Users.FirstAsync();

    var result = await repo.GetByUserNameAsync(user.UserName);

    result.Should().NotBeNull();
    result!.UserName.Should().Be(user.UserName);
  }

  [Fact]
  public async Task GetByUserNameAsync_Should_Return_Null_When_Not_Found()
  {
    var repo = new UserRepository(_context);

    var result = await repo.GetByUserNameAsync("nonExistentUser");

    result.Should().BeNull();
  }
}