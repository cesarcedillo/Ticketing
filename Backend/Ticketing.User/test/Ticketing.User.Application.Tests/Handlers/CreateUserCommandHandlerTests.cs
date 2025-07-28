using FluentAssertions;
using Moq;
using Ticketing.User.Application.Commands.CreateUser;
using Ticketing.User.Application.Dto.Responses;
using Ticketing.User.Application.Services.Interfaces;

namespace Ticketing.User.Application.Tests.Handlers;
public class CreateUserCommandHandlerTests
{
  private readonly Mock<IUserService> _userServiceMock;
  private readonly CreateUserCommandHandler _handler;

  public CreateUserCommandHandlerTests()
  {
    _userServiceMock = new Mock<IUserService>();
    _handler = new CreateUserCommandHandler(_userServiceMock.Object);
  }

  [Fact]
  public async Task Handle_Should_Call_CreateUserAsync_And_Return_UserResponse()
  {
    // Arrange
    var command = new CreateUserCommand("testuser", "avatar", "Admin");

    var expectedResponse = new UserResponse()
    {
      UserName = command.UserName,
      Avatar = command.Avatar,
      Type = command.Role
    };

    _userServiceMock
        .Setup(s => s.CreateUserAsync(command.UserName, command.Avatar, command.Role, It.IsAny<CancellationToken>()))
        .ReturnsAsync(expectedResponse);

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    result.Should().BeEquivalentTo(expectedResponse);
    _userServiceMock.Verify(s => s.CreateUserAsync(command.UserName, command.Avatar, command.Role, It.IsAny<CancellationToken>()), Times.Once);
  }

  [Fact]
  public async Task Handle_Should_Propagate_Exception_From_Service()
  {
    // Arrange
    var command = new CreateUserCommand("testuser", "avatar", "Admin");

    _userServiceMock
        .Setup(s => s.CreateUserAsync(command.UserName, command.Avatar, command.Role, It.IsAny<CancellationToken>()))
        .ThrowsAsync(new InvalidOperationException("User already exists."));

    // Act
    Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

    // Assert
    await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("User already exists.");
  }
}
