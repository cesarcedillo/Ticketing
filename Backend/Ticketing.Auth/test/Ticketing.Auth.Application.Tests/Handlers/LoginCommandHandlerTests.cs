using Moq;
using Ticketing.Auth.Application.Commands.Login;
using Ticketing.Auth.Application.Dtos.Responses;
using Ticketing.Auth.Application.Services.Interfaces;

namespace Ticketing.Auth.Application.Tests.Handlers;
public class LoginCommandHandlerTests
{
  private readonly Mock<IAuthService> _authServiceMock;
  private readonly LoginCommandHandler _handler;

  public LoginCommandHandlerTests()
  {
    _authServiceMock = new Mock<IAuthService>();
    _handler = new LoginCommandHandler(_authServiceMock.Object);
  }

  [Fact]
  public async Task Handle_Should_Return_Successful_LoginResponse()
  {
    // Arrange
    var userName = "testuser";
    var password = "Password123";
    var loginResponse = LoginResponse.SuccessResult("fakeToken", System.DateTime.UtcNow.AddHours(2), userName, "Admin");

    _authServiceMock
        .Setup(s => s.LoginAsync(userName, password, It.IsAny<CancellationToken>()))
        .ReturnsAsync(loginResponse);

    var command = new LoginCommand(userName, password);

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    Assert.NotNull(result);
    Assert.True(result.Success);
    Assert.Equal(userName, result.UserName);
    Assert.Equal("Admin", result.Role);
    Assert.Equal("fakeToken", result.AccessToken);
  }

  [Fact]
  public async Task Handle_Should_Return_Failure_LoginResponse_On_InvalidCredentials()
  {
    // Arrange
    var userName = "nouser";
    var password = "wrongPassword";
    var loginResponse = LoginResponse.Failure("Invalid credentials");

    _authServiceMock
        .Setup(s => s.LoginAsync(userName, password, It.IsAny<CancellationToken>()))
        .ReturnsAsync(loginResponse);

    var command = new LoginCommand(userName, password);

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    Assert.NotNull(result);
    Assert.False(result.Success);
    Assert.Equal("Invalid credentials", result.Message);
  }
}
