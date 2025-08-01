using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Ticketing.BFF.Application.Commands.User.CreateUser;
using Ticketing.BFF.Application.Dto.Responses;
using Ticketing.BFF.Application.Services.Interfaces;
using Xunit;

namespace Ticketing.BFF.Tests.Application.Commands.User
{
  public class CreateUserCommandBffHandlerTests
  {
    private readonly Mock<IUserService> _userServiceMock;
    private readonly CreateUserCommandBffHandler _handler;

    public CreateUserCommandBffHandlerTests()
    {
      _userServiceMock = new Mock<IUserService>();
      _handler = new CreateUserCommandBffHandler(_userServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCallCreateUserAsyncAndReturnResult()
    {
      // Arrange
      var command = new CreateUserCommandBff("testuser", "password123", "avatar.png", "admin");

      var expectedUser = new UserResponseBff
      {
        Id = Guid.NewGuid().ToString(),
        UserName = command.UserName,
        Avatar = command.Avatar,
        Type = command.Role
      };

      _userServiceMock
          .Setup(s => s.CreateUserAsync(command.UserName, command.Password, command.Avatar, command.Role, It.IsAny<CancellationToken>()))
          .ReturnsAsync(expectedUser);

      // Act
      var result = await _handler.Handle(command, CancellationToken.None);

      // Assert
      result.Should().BeEquivalentTo(expectedUser);
      _userServiceMock.Verify(s => s.CreateUserAsync(command.UserName, command.Password, command.Avatar, command.Role, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenUserServiceThrows_ShouldPropagateException()
    {
      // Arrange
      var command = new CreateUserCommandBff("testuser", "password123", "avatar.png", "admin");

      _userServiceMock
          .Setup(s => s.CreateUserAsync(command.UserName, command.Password, command.Avatar, command.Role, It.IsAny<CancellationToken>()))
          .ThrowsAsync(new InvalidOperationException("Something went wrong"));

      // Act
      var act = async () => await _handler.Handle(command, CancellationToken.None);

      // Assert
      await act.Should().ThrowAsync<InvalidOperationException>()
               .WithMessage("Something went wrong");

      _userServiceMock.Verify(s => s.CreateUserAsync(command.UserName, command.Password, command.Avatar, command.Role, It.IsAny<CancellationToken>()), Times.Once);
    }
  }
}
