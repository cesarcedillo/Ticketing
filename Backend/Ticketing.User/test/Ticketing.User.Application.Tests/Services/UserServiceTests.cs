using AutoMapper;
using FluentAssertions;
using Moq;
using System.Reflection.Metadata;
using Ticketing.User.Application.Dto.Responses;
using Ticketing.User.Application.Services;
using Ticketing.User.Domain.Interfaces.Repositories;
using Ticketing.User.TestCommon.Fixtures;
using UserType = Ticketing.User.Domain.Aggregates.User;

namespace Ticketing.User.Application.Tests.Services;
public class UserServiceTests
{
  private readonly Mock<IUserRepository> _userRepositoryMock;
  private readonly Mock<IMapper> _mapperMock;
  private readonly UserService _service;
  private readonly DomainFixture _domainFixture = new DomainFixture();

  public UserServiceTests()
  {
    _userRepositoryMock = new Mock<IUserRepository>();
    _mapperMock = new Mock<IMapper>();
    _service = new UserService(_userRepositoryMock.Object, _mapperMock.Object);
  }

  [Fact]
  public async Task GetUserByUserNameAsync_UserExists_ReturnsMappedUser()
  {
    // Arrange
    var userName = "testuser";
    var userEntity = _domainFixture.CreateDefaultAgent(userName);
    var userResponse = new UserResponse { UserName = userName, Avatar = userEntity.Avatar, Type = userEntity.UserType.ToString() };

    _userRepositoryMock
        .Setup(repo => repo.GetByUserNameAsync(userName, It.IsAny<CancellationToken>()))
        .ReturnsAsync(userEntity);

    _mapperMock
        .Setup(mapper => mapper.Map<UserResponse>(userEntity))
        .Returns(userResponse);

    // Act
    var result = await _service.GetUserByUserNameAsync(userName, CancellationToken.None);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(userResponse.UserName, result.UserName);
    Assert.Equal(userResponse.Avatar, result.Avatar);
    Assert.Equal(userResponse.Type, result.Type);
  }

  [Fact]
  public async Task GetUserByUserNameAsync_UserDoesNotExist_Throw_KeyNotFoundException()
  {
    // Arrange
    var userName = "notfound";
    _userRepositoryMock
        .Setup(repo => repo.GetByUserNameAsync(userName, It.IsAny<CancellationToken>()))
        .ReturnsAsync((UserType)null);

    // Act
    Func<Task> act = async () => await _service.GetUserByUserNameAsync(userName, CancellationToken.None);

    // Assert
    await act.Should().ThrowAsync<KeyNotFoundException>()
        .WithMessage($"User {userName} not found.");
  }
}
