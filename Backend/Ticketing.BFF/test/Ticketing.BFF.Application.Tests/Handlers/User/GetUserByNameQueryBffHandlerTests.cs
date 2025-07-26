using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Ticketing.BFF.Application.Dto.Responses;
using Ticketing.BFF.Application.Querires.User.GetUserByName;
using Ticketing.BFF.Application.Services.Interfaces;

namespace Ticketing.BFF.Tests.Application.Querires.User.GetUserByName
{
  public class GetUserByNameQueryBffHandlerTests
  {
    [Fact]
    public async Task Handle_ReturnsUserResponse_WhenUserExists()
    {
      // Arrange
      var userServiceMock = new Mock<IUserService>();
      var handler = new GetUserByNameQueryBffHandler(userServiceMock.Object);

      var query = new GetUserByNameQueryBff("Admin");

      var expectedUser = new UserResponseBff { UserName = "Admin" };

      userServiceMock
          .Setup(s => s.GetUserByUserNameAsync(query.UserName, It.IsAny<CancellationToken>()))
          .ReturnsAsync(expectedUser);

      // Act
      var result = await handler.Handle(query, CancellationToken.None);

      // Assert
      Assert.Same(expectedUser, result);
      userServiceMock.Verify(
          s => s.GetUserByUserNameAsync(query.UserName, It.IsAny<CancellationToken>()),
          Times.Once
      );
    }

    [Fact]
    public async Task Handle_ReturnsNull_WhenUserDoesNotExist()
    {
      // Arrange
      var userServiceMock = new Mock<IUserService>();
      var handler = new GetUserByNameQueryBffHandler(userServiceMock.Object);

      var query = new GetUserByNameQueryBff("nouser");

      userServiceMock
          .Setup(s => s.GetUserByUserNameAsync(query.UserName, It.IsAny<CancellationToken>()))
          .ReturnsAsync((UserResponseBff)null);

      // Act
      var result = await handler.Handle(query, CancellationToken.None);

      // Assert
      Assert.Null(result);
      userServiceMock.Verify(
          s => s.GetUserByUserNameAsync(query.UserName, It.IsAny<CancellationToken>()),
          Times.Once
      );
    }
  }
}
