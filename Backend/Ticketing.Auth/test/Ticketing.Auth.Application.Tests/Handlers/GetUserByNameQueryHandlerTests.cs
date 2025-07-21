using Moq;
using Ticketing.Auth.Application.Dtos.Responses;
using Ticketing.Auth.Application.Queries.GetUserByName;
using Ticketing.Auth.Application.Services.Interfaces;

namespace Ticketing.Auth.Application.Tests.Handlers;
public class GetUserByNameQueryHandlerTests
{
  private readonly Mock<IUserService> _userServiceMock;
  private readonly GetUserByNameQueryHandler _handler;

  public GetUserByNameQueryHandlerTests()
  {
    _userServiceMock = new Mock<IUserService>();
    _handler = new GetUserByNameQueryHandler(_userServiceMock.Object);
  }

  [Fact]
  public async Task Handle_Should_Return_UserResponse_When_User_Exists()
  {
    // Arrange
    var userName = "testuser";
    var userResponse = new UserResponse
    {
      UserName = userName,
      Role = "Admin"
    };

    _userServiceMock
        .Setup(s => s.GetUserByUserNameAsync(userName, It.IsAny<CancellationToken>()))
        .ReturnsAsync(userResponse);

    var query = new GetUserByNameQuery(userName);

    // Act
    var result = await _handler.Handle(query, CancellationToken.None);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(userName, result.UserName);
    Assert.Equal("Admin", result.Role);
  }
}