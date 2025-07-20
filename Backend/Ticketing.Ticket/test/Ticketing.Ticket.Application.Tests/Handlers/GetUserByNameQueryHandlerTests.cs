using Azure.Core;
using FluentAssertions;
using Moq;
using Ticketing.Ticket.Application.Dtos.Responses;
using Ticketing.Ticket.Application.Queries.GetTicketDetail;
using Ticketing.Ticket.Application.Queries.GetUserByName;
using Ticketing.Ticket.Application.Services.Interfaces;

namespace Ticketing.Ticket.Application.Tests.Handlers
{
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
    public async Task Handle_Should_Return_DetailDto_When_Service_Returns_Result()
    {
      // Arrange
      var userName = "userName";
      var expectedDto = new UserResponse { UserName = userName };
      _userServiceMock
          .Setup(s => s.GetUserByUserNameAsync(userName, It.IsAny<CancellationToken>()))
          .ReturnsAsync(expectedDto);

      var query = new GetUserByNameQuery(userName);

      // Act
      var result = await _handler.Handle(query, CancellationToken.None);

      // Assert
      result.Should().Be(expectedDto);
      _userServiceMock.Verify(s =>
          s.GetUserByUserNameAsync(query.UserName, It.IsAny<CancellationToken>()),
          Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_KeyNotFoundException_When_Service_Returns_Null()
    {
      // Arrange
      var userName = "nonExistentUser";
      _userServiceMock
          .Setup(s => s.GetUserByUserNameAsync(userName, It.IsAny<CancellationToken>()))
          .ThrowsAsync(new KeyNotFoundException($"User {userName} not found."));

      var query = new GetUserByNameQuery(userName);

      // Act
      Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

      // Assert
      await act.Should().ThrowAsync<KeyNotFoundException>()
          .WithMessage($"User {userName} not found.");
    }


    [Fact]
    public async Task Handle_Should_Propagate_Exception_If_Service_Fails()
    {
      // Arrange
      var userName = "userName";
      _userServiceMock
          .Setup(s => s.GetUserByUserNameAsync(userName, It.IsAny<CancellationToken>()))
          .ThrowsAsync(new InvalidOperationException("Some error"));

      var query = new GetUserByNameQuery(userName);

      // Act
      Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

      // Assert
      await act.Should().ThrowAsync<InvalidOperationException>()
          .WithMessage("Some error");
    }
  }
}
