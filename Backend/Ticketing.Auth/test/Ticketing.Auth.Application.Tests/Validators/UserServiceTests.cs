
using AutoMapper;
using FluentAssertions;
using Moq;
using Ticketing.Auth.Application.Dtos.Responses;
using Ticketing.Auth.Application.Services;
using Ticketing.Auth.Domain.Aggregates;
using Ticketing.Auth.Domain.Enums;
using Ticketing.Auth.Domain.Interfaces;

namespace Ticketing.Auth.Application.Tests.Validators;

public class UserServiceTests
{
  private readonly Mock<IUserRepository> _userRepositoryMock;
  private readonly Mock<IMapper> _mapperMock;
  private readonly UserService _userService;

  public UserServiceTests()
  {
    _userRepositoryMock = new Mock<IUserRepository>();
    _mapperMock = new Mock<IMapper>();
    _userService = new UserService(_userRepositoryMock.Object, _mapperMock.Object);
  }

  [Fact]
  public async Task GetUserByUserNameAsync_Should_Return_UserResponse_When_UserExists()
  {
    // Arrange
    var userName = "alice";
    var user = new User(userName, "hash", Role.Customer);
    var userResponse = new UserResponse
    {
      UserName = userName,
      Role = "Customer"
    };

    _userRepositoryMock.Setup(repo => repo.GetByUserNameAsync(userName, It.IsAny<CancellationToken>()))
        .ReturnsAsync(user);

    _mapperMock.Setup(mapper => mapper.Map<UserResponse>(user)).Returns(userResponse);

    // Act
    var result = await _userService.GetUserByUserNameAsync(userName, CancellationToken.None);

    // Assert
    result.Should().BeEquivalentTo(userResponse);
    _userRepositoryMock.Verify(r => r.GetByUserNameAsync(userName, It.IsAny<CancellationToken>()), Times.Once);
    _mapperMock.Verify(m => m.Map<UserResponse>(user), Times.Once);
  }

  [Fact]
  public async Task GetUserByUserNameAsync_Should_Throw_When_UserDoesNotExist()
  {
    // Arrange
    var userName = "notfound";
    _userRepositoryMock.Setup(repo => repo.GetByUserNameAsync(userName, It.IsAny<CancellationToken>()))
        .ReturnsAsync((User)null);

    // Act
    Func<Task> act = async () => await _userService.GetUserByUserNameAsync(userName, CancellationToken.None);

    // Assert
    await act.Should().ThrowAsync<KeyNotFoundException>()
        .WithMessage($"User {userName} not found.");
    _userRepositoryMock.Verify(r => r.GetByUserNameAsync(userName, It.IsAny<CancellationToken>()), Times.Once);
    _mapperMock.Verify(m => m.Map<UserResponse>(It.IsAny<User>()), Times.Never);
  }
}
