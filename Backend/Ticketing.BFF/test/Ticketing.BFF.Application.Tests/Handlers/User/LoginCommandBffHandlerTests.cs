using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Ticketing.BFF.Application.Commands.User.Login;
using Ticketing.BFF.Application.Dto.Responses;
using Ticketing.BFF.Application.Services.Interfaces;

namespace Ticketing.BFF.Tests.Application.Commands.User.Login
{
  public class LoginCommandBffHandlerTests
  {
    [Fact]
    public async Task Handle_CallsUserServiceAndReturnsResult()
    {
      // Arrange
      var userServiceMock = new Mock<IUserService>();
      var handler = new LoginCommandBffHandler(userServiceMock.Object);

      var command = new LoginCommandBff("admin", "1234");

      var expectedResponse = new LoginResponseBff { Success = true, AccessToken = "token" };

      userServiceMock
          .Setup(s => s.LoginAsync(command.UserName, command.Password, It.IsAny<CancellationToken>()))
          .ReturnsAsync(expectedResponse);

      // Act
      var result = await handler.Handle(command, CancellationToken.None);

      // Assert
      Assert.Same(expectedResponse, result);
      userServiceMock.Verify(
          s => s.LoginAsync(command.UserName, command.Password, It.IsAny<CancellationToken>()),
          Times.Once
      );
    }

    [Fact]
    public async Task Handle_ReturnsNull_WhenUserServiceReturnsNull()
    {
      // Arrange
      var userServiceMock = new Mock<IUserService>();
      var handler = new LoginCommandBffHandler(userServiceMock.Object);

      var command = new LoginCommandBff("nouser", "nopass");

      userServiceMock
          .Setup(s => s.LoginAsync(command.UserName, command.Password, It.IsAny<CancellationToken>()))
          .ReturnsAsync((LoginResponseBff)null);

      // Act
      var result = await handler.Handle(command, CancellationToken.None);

      // Assert
      Assert.Null(result);
      userServiceMock.Verify(
          s => s.LoginAsync(command.UserName, command.Password, It.IsAny<CancellationToken>()),
          Times.Once
      );
    }
  }
}
