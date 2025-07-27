using Moq;
using Ticketing.BFF.Application.Dto.Responses;
using Ticketing.BFF.Application.Querires.Ticket.GetTicketDetail;
using Ticketing.BFF.Application.Services.Interfaces;

namespace Ticketing.BFF.Application.Tests.Handlers.Ticket;
public class GetTicketDetailQueryBffHandlerTests
{
  [Fact]
  public async Task Handle_ReturnsTicketDetailResponseBff_WhenTicketExists()
  {
    // Arrange
    var ticketServiceMock = new Mock<ITicketService>();
    var handler = new GetTicketDetailQueryBffHandler(ticketServiceMock.Object);

    var ticketId = Guid.NewGuid();
    var expectedDetail = new TicketDetailResponseBff { Id = ticketId };

    var query = new GetTicketDetailQueryBff(ticketId);

    ticketServiceMock
        .Setup(s => s.GetTicketDetailAsync(ticketId, It.IsAny<CancellationToken>()))
        .ReturnsAsync(expectedDetail);

    // Act
    var result = await handler.Handle(query, CancellationToken.None);

    // Assert
    Assert.NotNull(result);
    Assert.Same(expectedDetail, result);
    ticketServiceMock.Verify(
        s => s.GetTicketDetailAsync(ticketId, It.IsAny<CancellationToken>()),
        Times.Once
    );
  }
}
