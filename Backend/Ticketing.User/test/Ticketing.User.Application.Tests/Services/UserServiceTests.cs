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

  [Fact]
  public async Task GetUsersByIdsAsync_UsersExist_ReturnsMappedUsers()
  {
    // Arrange
    var userNames = new[] { "UserName1", "UserName2" };

    var userEntities = userNames.Select(userName => _domainFixture.CreateDefaultAgent(userName)).ToList();
    var userIds = userEntities.Select(u => u.Id).ToList();
    var userResponses = userEntities.Select(u => new UserResponse
    {
      Id = u.Id,
      UserName = u.UserName,
      Avatar = u.Avatar,
      Type = u.UserType.ToString()
    }).ToList();

    _userRepositoryMock
        .Setup(repo => repo.GetByIdsAsync(userIds, It.IsAny<CancellationToken>()))
        .ReturnsAsync(userEntities);

    for (int i = 0; i < userEntities.Count; i++)
    {
      _mapperMock
          .Setup(mapper => mapper.Map<UserResponse>(userEntities[i]))
          .Returns(userResponses[i]);
    }

    // Act
    var result = (await _service.GetUsersByIdsAsync(userIds, CancellationToken.None)).ToList();

    // Assert
    result.Should().NotBeNull();
    result.Should().HaveCount(userEntities.Count);
    result.Select(r => r.Id).Should().BeEquivalentTo(userIds);
  }

  [Fact]
  public async Task GetUsersByIdsAsync_NoUsersExist_ReturnsEmpty()
  {
    // Arrange
    var userIds = new[] { Guid.NewGuid(), Guid.NewGuid() };

    _userRepositoryMock
        .Setup(repo => repo.GetByIdsAsync(userIds, It.IsAny<CancellationToken>()))
        .ReturnsAsync(new List<UserType>());

    // Act
    var result = await _service.GetUsersByIdsAsync(userIds, CancellationToken.None);

    // Assert
    result.Should().NotBeNull();
    result.Should().BeEmpty();
  }

}
