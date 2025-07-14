using FluentAssertions;
using Moq;
using Ticketing.Application.Commands.MarkTicketAsResolved;
using Ticketing.Application.Services.Interfaces;

namespace Ticketing.Application.Tests.Handlers
{
  public class MarkTicketAsResolvedCommandHandlerTests
  {
    private readonly Mock<ITicketService> _ticketServiceMock;
    private readonly MarkTicketAsResolvedCommandHandler _handler;

    public MarkTicketAsResolvedCommandHandlerTests()
    {
      _ticketServiceMock = new Mock<ITicketService>();
      _handler = new MarkTicketAsResolvedCommandHandler(_ticketServiceMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Call_Service()
    {
      // Arrange
      var ticketId = Guid.NewGuid();
      _ticketServiceMock
          .Setup(s => s.MarkTicketAsResolvedAsync(ticketId, It.IsAny<CancellationToken>()))
          .Returns(Task.CompletedTask);

      var command = new MarkTicketAsResolvedCommand(ticketId);

      // Act
      await _handler.Handle(command, CancellationToken.None);

      // Assert
      _ticketServiceMock.Verify(s =>
          s.MarkTicketAsResolvedAsync(command.TicketId, It.IsAny<CancellationToken>()),
          Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Propagate_Exception_If_Service_Fails()
    {
      // Arrange
      var ticketId = Guid.NewGuid();
      _ticketServiceMock
          .Setup(s => s.MarkTicketAsResolvedAsync(ticketId, It.IsAny<CancellationToken>()))
          .ThrowsAsync(new InvalidOperationException("Some error"));

      var command = new MarkTicketAsResolvedCommand(ticketId);

      // Act
      Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

      // Assert
      await act.Should().ThrowAsync<InvalidOperationException>()
          .WithMessage("Some error");
    }
  }
}
