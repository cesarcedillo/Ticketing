using FluentAssertions;
using Moq;
using Ticketing.Notifications.Application.Commands.TicketResolved;
using Ticketing.Notifications.Application.Services.Interfaces;

namespace Ticketing.Notifications.Application.Tests.Handlers;
public class TicketResolvedCommandHandlerTests
{
  private readonly Mock<INotificationsService> _notificationServiceMock;
  private readonly TicketResolvedCommandHandler _handler;

  public TicketResolvedCommandHandlerTests()
  {
    _notificationServiceMock = new Mock<INotificationsService>();
    _handler = new TicketResolvedCommandHandler(_notificationServiceMock.Object);
  }

  [Fact]
  public async Task Handle_Should_Call_NotifyTicketResolvedAsync_With_Correct_Parameters()
  {
    // Arrange
    var ticketId = Guid.NewGuid();
    var userId = Guid.NewGuid();
    var command = new TicketResolvedCommand(ticketId, userId);
    var cancellationToken = CancellationToken.None;

    // Act
    await _handler.Handle(command, cancellationToken);

    // Assert
    _notificationServiceMock.Verify(
        s => s.NotifyTicketResolvedAsync(ticketId, userId, cancellationToken),
        Times.Once);
  }

  [Fact]
  public async Task Handle_Should_Complete_Without_Exception()
  {
    // Arrange
    var command = new TicketResolvedCommand(Guid.NewGuid(), Guid.NewGuid());

    // Act
    var act = () => _handler.Handle(command, CancellationToken.None);

    // Assert
    await act.Should().NotThrowAsync();
  }

  [Fact]
  public async Task Handle_Should_Propagate_Exception_When_Service_Fails()
  {
    // Arrange
    var ticketId = Guid.NewGuid();
    var userId = Guid.NewGuid();
    var command = new TicketResolvedCommand(ticketId, userId);

    _notificationServiceMock
        .Setup(s => s.NotifyTicketResolvedAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
        .ThrowsAsync(new InvalidOperationException("Error when notifying"));

    // Act
    Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

    // Assert
    await act.Should().ThrowAsync<InvalidOperationException>()
             .WithMessage("Error when notifying");
  }
}
