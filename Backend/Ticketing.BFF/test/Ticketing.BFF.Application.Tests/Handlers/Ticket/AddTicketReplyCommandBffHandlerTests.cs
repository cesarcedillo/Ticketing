using Moq;
using Ticketing.BFF.Application.Commands.Ticket.AddTicketReply;
using Ticketing.BFF.Application.Services.Interfaces;

namespace Ticketing.BFF.Application.Tests.Handlers.Ticket;
public class AddTicketReplyCommandBffHandlerTests
{
  [Fact]
  public async Task Handle_CallsAddReplyAsync_WithCorrectParameters()
  {
    // Arrange
    var ticketServiceMock = new Mock<ITicketService>();
    var handler = new AddTicketReplyCommandBffHandler(ticketServiceMock.Object);

    var ticketId = Guid.NewGuid();
    var userId = Guid.NewGuid();
    var text = "comment random";

    var command = new AddTicketReplyCommandBff(ticketId, text, userId);

    // Act
    await handler.Handle(command, CancellationToken.None);

    // Assert
    ticketServiceMock.Verify(
        s => s.AddReplyAsync(ticketId, text, userId, It.IsAny<CancellationToken>()),
        Times.Once
    );
  }

  [Fact]
  public async Task Handle_PropagatesCancellationToken()
  {
    // Arrange
    var ticketServiceMock = new Mock<ITicketService>();
    var handler = new AddTicketReplyCommandBffHandler(ticketServiceMock.Object);

    var ticketId = Guid.NewGuid();
    var userId = Guid.NewGuid();
    var text = "another comment";
    var cts = new CancellationTokenSource();

    var command = new AddTicketReplyCommandBff(ticketId, text, userId);

    // Act
    await handler.Handle(command, cts.Token);

    // Assert
    ticketServiceMock.Verify(
        s => s.AddReplyAsync(ticketId, text, userId, cts.Token),
        Times.Once
    );
  }
}
