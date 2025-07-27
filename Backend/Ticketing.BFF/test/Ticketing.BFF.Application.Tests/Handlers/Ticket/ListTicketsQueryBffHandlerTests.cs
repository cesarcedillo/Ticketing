using Moq;
using Ticketing.BFF.Application.Dto.Responses;
using Ticketing.BFF.Application.Querires.Ticket.ListTickets;
using Ticketing.BFF.Application.Services.Interfaces;

namespace Ticketing.BFF.Application.Tests.Handlers.Ticket;
public class ListTicketsQueryBffHandlerTests
{
  [Fact]
  public async Task Handle_ReturnsList_WhenTicketsExist()
  {
    // Arrange
    var ticketServiceMock = new Mock<ITicketService>();
    var handler = new ListTicketsQueryBffHandler(ticketServiceMock.Object);

    string? status = "Open";
    Guid? userId = Guid.NewGuid();
    var expectedList = new List<TicketResponseBff>
            {
                new TicketResponseBff { Id = Guid.NewGuid() },
                new TicketResponseBff { Id = Guid.NewGuid() }
            };

    var query = new ListTicketsQueryBff(status, userId);

    ticketServiceMock
        .Setup(s => s.ListTicketsAsync(status, userId, It.IsAny<CancellationToken>()))
        .ReturnsAsync(expectedList);

    // Act
    var result = await handler.Handle(query, CancellationToken.None);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(expectedList, result);
    ticketServiceMock.Verify(
        s => s.ListTicketsAsync(status, userId, It.IsAny<CancellationToken>()),
        Times.Once
    );
  }

  [Fact]
  public async Task Handle_ReturnsEmptyList_WhenNoTicketsExist()
  {
    // Arrange
    var ticketServiceMock = new Mock<ITicketService>();
    var handler = new ListTicketsQueryBffHandler(ticketServiceMock.Object);

    string? status = null;
    Guid? userId = null;
    var expectedList = new List<TicketResponseBff>();

    var query = new ListTicketsQueryBff(status, userId);

    ticketServiceMock
        .Setup(s => s.ListTicketsAsync(status, userId, It.IsAny<CancellationToken>()))
        .ReturnsAsync(expectedList);

    // Act
    var result = await handler.Handle(query, CancellationToken.None);

    // Assert
    Assert.NotNull(result);
    Assert.Empty(result);
    ticketServiceMock.Verify(
        s => s.ListTicketsAsync(status, userId, It.IsAny<CancellationToken>()),
        Times.Once
    );
  }
}
