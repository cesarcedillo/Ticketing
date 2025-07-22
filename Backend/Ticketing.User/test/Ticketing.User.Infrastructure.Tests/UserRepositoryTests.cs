using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Ticketing.User.Domain.Enums;
using Ticketing.User.Infraestructure.Data;
using Ticketing.User.Infraestructure.Data.Repositories;
using UserType = Ticketing.User.Domain.Aggregates.User;

namespace Ticketing.User.Infrastructure.Tests;
public class UserRepositoryTests
{
  private readonly UserDbContext _context;

  public UserRepositoryTests()
  {
    var contextOptions = new DbContextOptionsBuilder<UserDbContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString())
        .Options;
    _context = new UserDbContext(contextOptions);

    _context.Users.RemoveRange(_context.Users);
    _context.SaveChanges();

    var user1 = new UserType("alice", "avatar", Role.Customer);
    var user2 = new UserType("bob", "avatar", Role.Agent);

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
