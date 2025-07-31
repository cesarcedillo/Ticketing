using FluentAssertions;
using Moq;
using Ticketing.Auth.Application.Commands.SignUp;
using Ticketing.Auth.Application.Dtos.Responses;
using Ticketing.Auth.Application.Services.Interfaces;
using Ticketing.Auth.Domain.Enums;

namespace Ticketing.Auth.Application.Tests.Handlers
{
  public class SignUpCommandHandlerTests
  {
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly SignUpCommandHandler _handler;

    public SignUpCommandHandlerTests()
    {
      _authServiceMock = new Mock<IAuthService>();
      _handler = new SignUpCommandHandler(_authServiceMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Call_SignUpAsync_And_Return_Response()
    {
      // Arrange
      var command = new SignUpCommand("user", "pass", Role.Agent.ToString());

      var expectedResponse = SignInResponse.SuccessResult(
          "token123",
          DateTime.UtcNow.AddHours(1),
          "user",
          "Agent"
      );

      _authServiceMock
          .Setup(x => x.SignUpAsync("user", "pass", Role.Agent, It.IsAny<CancellationToken>()))
          .ReturnsAsync(expectedResponse);

      // Act
      var result = await _handler.Handle(command, CancellationToken.None);

      // Assert
      result.Should().BeEquivalentTo(expectedResponse);
      _authServiceMock.Verify(x =>
          x.SignUpAsync("user", "pass", Role.Agent, It.IsAny<CancellationToken>()),
          Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Propagate_Exception_From_Service()
    {
      // Arrange
      var command = new SignUpCommand("existing", "pass", Role.Admin.ToString());

      _authServiceMock
          .Setup(x => x.SignUpAsync(command.UserName, command.Password, Role.Admin, It.IsAny<CancellationToken>()))
          .ThrowsAsync(new InvalidOperationException("User already exists"));

      // Act
      Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

      // Assert
      await act.Should().ThrowAsync<InvalidOperationException>()
          .WithMessage("User already exists");
    }
  }

}
