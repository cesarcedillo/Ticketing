using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Ticketing.Domain.Aggregates;
using Ticketing.Domain.Enums;
using Ticketing.Infrastructure.Data;
using Ticketing.Infrastructure.Data.Repositories;

namespace Ticketing.Infrastructure.Tests;
public class UserRepositoryTests
{
  private readonly TicketDbContext _context;

  public UserRepositoryTests()
  {
    var contextOptions = new DbContextOptionsBuilder<TicketDbContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString())
        .Options;
    _context = new TicketDbContext(contextOptions);

    _context.Users.RemoveRange(_context.Users);
    _context.SaveChanges();

    var user1 = new User("alice", "avatar", UserType.Customer);
    var user2 = new User("bob", "avatar", UserType.Agent);

    _context.Users.AddRange(user1, user2);
    _context.SaveChanges();
  }

  [Fact]
  public async Task GetByIdAsync_Should_Return_User_When_Exists()
  {
    var repo = new UserRepository(_context);

    var user = await _context.Users.FirstAsync();

    var result = await repo.GetByIdAsync(user.Id);

    result.Should().NotBeNull();
    result!.UserName.Should().Be(user.UserName);
  }

  [Fact]
  public async Task GetByIdAsync_Should_Return_Null_When_Not_Found()
  {
    var repo = new UserRepository(_context);

    var result = await repo.GetByIdAsync(Guid.NewGuid());

    result.Should().BeNull();
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
