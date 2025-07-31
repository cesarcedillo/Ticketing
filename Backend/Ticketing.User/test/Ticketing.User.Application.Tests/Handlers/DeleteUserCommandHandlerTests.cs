using FluentAssertions;
using MediatR;
using Moq;
using Ticketing.User.Application.Commands.DeleteUser;
using Ticketing.User.Application.Services.Interfaces;

namespace Ticketing.User.Application.Tests.Handlers;
public class DeleteUserCommandHandlerTests
{
  private readonly Mock<IUserService> _userServiceMock;
  private readonly DeleteUserCommandHandler _handler;

  public DeleteUserCommandHandlerTests()
  {
    _userServiceMock = new Mock<IUserService>();
    _handler = new DeleteUserCommandHandler(_userServiceMock.Object);
  }

  [Fact]
  public async Task Handle_Should_Call_DeleteUserAsync_And_Return_Unit()
  {
    // Arrange
    var userName = "testuser";
    var command = new DeleteUserCommand(userName);

    _userServiceMock
        .Setup(service => service.DeleteUserAsync(userName, It.IsAny<CancellationToken>()))
        .Returns(Task.CompletedTask)
        .Verifiable();

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    result.Should().Be(Unit.Value);
    _userServiceMock.Verify(service => service.DeleteUserAsync(userName, It.IsAny<CancellationToken>()), Times.Once);
  }

  [Fact]
  public async Task Handle_Should_Propagate_Exception_When_DeleteUserAsync_Fails()
  {
    // Arrange
    var userName = "invaliduser";
    var command = new DeleteUserCommand(userName);

    _userServiceMock
        .Setup(service => service.DeleteUserAsync(userName, It.IsAny<CancellationToken>()))
        .ThrowsAsync(new KeyNotFoundException("User not found."));

    // Act
    Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

    // Assert
    await act.Should().ThrowAsync<KeyNotFoundException>()
        .WithMessage("User not found.");
  }
}

