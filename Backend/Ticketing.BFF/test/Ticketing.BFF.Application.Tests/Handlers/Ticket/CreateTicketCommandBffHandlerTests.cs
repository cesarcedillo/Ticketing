using Moq;
using Ticketing.BFF.Application.Commands.Ticket.CreateTicket;
using Ticketing.BFF.Application.Services.Interfaces;

namespace Ticketing.BFF.Application.Tests.Handlers.Ticket;
public class CreateTicketCommandBffHandlerTests
{
  [Fact]
  public async Task Handle_CallsServiceAndReturnsCreateTicketResponse()
  {
    // Arrange
    var ticketServiceMock = new Mock<ITicketService>();
    var handler = new CreateTicketCommandBffHandler(ticketServiceMock.Object);

    var subject = "Test Subject";
    var description = "Test Description";
    var userId = Guid.NewGuid();
    var expectedTicketId = Guid.NewGuid();

    var command = new CreateTicketCommandBff(subject, description, userId);

    ticketServiceMock
        .Setup(s => s.CreateTicketAsync(subject, description, userId, It.IsAny<CancellationToken>()))
        .ReturnsAsync(expectedTicketId);

    // Act
    var result = await handler.Handle(command, CancellationToken.None);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(expectedTicketId, result.TicketId);
    ticketServiceMock.Verify(
        s => s.CreateTicketAsync(subject, description, userId, It.IsAny<CancellationToken>()),
        Times.Once
    );
  }

  [Fact]
  public async Task Handle_PropagatesCancellationToken()
  {
    // Arrange
    var ticketServiceMock = new Mock<ITicketService>();
    var handler = new CreateTicketCommandBffHandler(ticketServiceMock.Object);

    var subject = "S";
    var description = "D";
    var userId = Guid.NewGuid();
    var expectedTicketId = Guid.NewGuid();
    var cts = new CancellationTokenSource();

    var command = new CreateTicketCommandBff(subject, description, userId);

    ticketServiceMock
        .Setup(s => s.CreateTicketAsync(subject, description, userId, cts.Token))
        .ReturnsAsync(expectedTicketId);

    // Act
    var result = await handler.Handle(command, cts.Token);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(expectedTicketId, result.TicketId);
    ticketServiceMock.Verify(
        s => s.CreateTicketAsync(subject, description, userId, cts.Token),
        Times.Once
    );
  }
}
