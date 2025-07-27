using Moq;
using Ticketing.BFF.Application.Commands.Ticket.MarkTicketAsResolved;
using Ticketing.BFF.Application.Services.Interfaces;

namespace Ticketing.BFF.Application.Tests.Handlers.Ticket;
public class MarkTicketAsResolvedCommandBffHandlerTests
{
  [Fact]
  public async Task Handle_CallsMarkTicketAsResolvedAsync_WithCorrectParameters()
  {
    // Arrange
    var ticketServiceMock = new Mock<ITicketService>();
    var handler = new MarkTicketAsResolvedCommandBffHandler(ticketServiceMock.Object);

    var ticketId = Guid.NewGuid();

    var command = new MarkTicketAsResolvedCommandBff(ticketId);

    // Act
    await handler.Handle(command, CancellationToken.None);

    // Assert
    ticketServiceMock.Verify(
        s => s.MarkTicketAsResolvedAsync(ticketId, It.IsAny<CancellationToken>()),
        Times.Once
    );
  }

  [Fact]
  public async Task Handle_PropagatesCancellationToken()
  {
    // Arrange
    var ticketServiceMock = new Mock<ITicketService>();
    var handler = new MarkTicketAsResolvedCommandBffHandler(ticketServiceMock.Object);

    var ticketId = Guid.NewGuid();
    var cts = new CancellationTokenSource();

    var command = new MarkTicketAsResolvedCommandBff(ticketId);

    // Act
    await handler.Handle(command, cts.Token);

    // Assert
    ticketServiceMock.Verify(
        s => s.MarkTicketAsResolvedAsync(ticketId, cts.Token),
        Times.Once
    );
  }
}
