using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Ticketing.User.Domain.Enums;
using Ticketing.User.Infrastructure.Data;
using Ticketing.User.Infrastructure.Data.Repositories;
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

  [Fact]
  public async Task GetByIdsAsync_Should_Return_Users_When_Exist()
  {
    var repo = new UserRepository(_context);

    var users = _context.Users.ToList();
    var ids = users.Select(u => u.Id);

    var result = await repo.GetByIdsAsync(ids);

    result.Should().NotBeNull();
    result.Should().HaveCount(users.Count);
    result.Select(u => u.Id).Should().BeEquivalentTo(ids);
  }

  [Fact]
  public async Task GetByIdsAsync_Should_Return_Empty_When_None_Exist()
  {
    var repo = new UserRepository(_context);

    var ids = new[] { Guid.NewGuid(), Guid.NewGuid() };

    var result = await repo.GetByIdsAsync(ids);

    result.Should().NotBeNull();
    result.Should().BeEmpty();
  }

  [Fact]
  public async Task GetByIdsAsync_Should_Return_Partial_Results_When_Some_Exist()
  {
    var repo = new UserRepository(_context);

    var existingUser = await _context.Users.FirstAsync();
    var ids = new[] { existingUser.Id, Guid.NewGuid() };

    var result = await repo.GetByIdsAsync(ids);

    result.Should().NotBeNull();
    result.Should().HaveCount(1);
    result.First().Id.Should().Be(existingUser.Id);
  }

  [Fact]
  public async Task DeleteAsync_Should_Remove_User_When_Exists()
  {
    // Arrange
    var repo = new UserRepository(_context);
    var user = await _context.Users.FirstAsync();

    // Act
    await repo.DeleteAsync(user.Id);
    await _context.SaveChangesAsync(); 

    // Assert
    var deletedUser = await _context.Users.FindAsync(user.Id);
    deletedUser.Should().BeNull();
  }

  [Fact]
  public async Task DeleteAsync_Should_Not_Throw_When_User_Does_Not_Exist()
  {
    // Arrange
    var repo = new UserRepository(_context);
    var nonExistentId = Guid.NewGuid();

    // Act
    Func<Task> act = async () =>
    {
      await repo.DeleteAsync(nonExistentId);
      await _context.SaveChangesAsync();
    };

    // Assert
    await act.Should().NotThrowAsync();
  }

}
