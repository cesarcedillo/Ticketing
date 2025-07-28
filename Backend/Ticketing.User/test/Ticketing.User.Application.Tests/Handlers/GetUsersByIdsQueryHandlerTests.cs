using FluentAssertions;
using Moq;
using Ticketing.User.Application.Dto.Responses;
using Ticketing.User.Application.Queries.GetUsersByIds;
using Ticketing.User.Application.Services.Interfaces;

namespace Ticketing.User.Application.Tests.Handlers;
public class GetUsersByIdsQueryHandlerTests
{
  private readonly Mock<IUserService> _userServiceMock;
  private readonly GetUsersByIdsQueryHandler _handler;

  public GetUsersByIdsQueryHandlerTests()
  {
    _userServiceMock = new Mock<IUserService>();
    _handler = new GetUsersByIdsQueryHandler(_userServiceMock.Object);
  }

  [Fact]
  public async Task Handle_UsersExist_ReturnsUserResponses()
  {
    // Arrange
    var userIds = new[] { Guid.NewGuid(), Guid.NewGuid() };
    var users = userIds.Select(id => new UserResponse { Id = id, UserName = $"user_{id}" }).ToList();

    _userServiceMock
        .Setup(s => s.GetUsersByIdsAsync(userIds, It.IsAny<CancellationToken>()))
        .ReturnsAsync(users);

    var query = new GetUsersByIdsQuery(userIds);

    // Act
    var result = await _handler.Handle(query, CancellationToken.None);

    // Assert
    result.Should().NotBeNull();
    result.Should().HaveCount(userIds.Length);
    result.Select(u => u.Id).Should().BeEquivalentTo(userIds);
  }

  [Fact]
  public async Task Handle_NoUsersExist_ReturnsEmptyList()
  {
    // Arrange
    var userIds = new[] { Guid.NewGuid(), Guid.NewGuid() };

    _userServiceMock
        .Setup(s => s.GetUsersByIdsAsync(userIds, It.IsAny<CancellationToken>()))
        .ReturnsAsync(new List<UserResponse>());

    var query = new GetUsersByIdsQuery(userIds);

    // Act
    var result = await _handler.Handle(query, CancellationToken.None);

    // Assert
    result.Should().NotBeNull();
    result.Should().BeEmpty();
  }
}
