using FluentAssertions;
using Moq;
using Ticketing.Ticket.Application.Dtos.Responses;
using Ticketing.Ticket.Application.Queries.GetTicketDetail;
using Ticketing.Ticket.Application.Services.Interfaces;

namespace Ticketing.Ticket.Application.Tests.Handlers;
  public class GetTicketDetailQueryHandlerTests
{
  private readonly Mock<ITicketService> _ticketServiceMock;
  private readonly GetTicketDetailQueryHandler _handler;

  public GetTicketDetailQueryHandlerTests()
  {
    _ticketServiceMock = new Mock<ITicketService>();
    _handler = new GetTicketDetailQueryHandler(_ticketServiceMock.Object);
  }

  [Fact]
  public async Task Handle_Should_Return_DetailDto_When_Service_Returns_Result()
  {
    // Arrange
    var ticketId = Guid.NewGuid();
    var expectedDto = new TicketDetailResponse { Id = ticketId };
    _ticketServiceMock
        .Setup(s => s.GetTicketDetailAsync(ticketId, It.IsAny<CancellationToken>()))
        .ReturnsAsync(expectedDto);

    var query = new GetTicketDetailQuery(ticketId);

    // Act
    var result = await _handler.Handle(query, CancellationToken.None);

    // Assert
    result.Should().Be(expectedDto);
    _ticketServiceMock.Verify(s =>
        s.GetTicketDetailAsync(query.TicketId, It.IsAny<CancellationToken>()),
        Times.Once);
  }

  [Fact]
  public async Task Handle_Should_Throw_KeyNotFoundException_When_Service_Returns_Null()
  {
    // Arrange
    var ticketId = Guid.NewGuid();
    _ticketServiceMock
        .Setup(s => s.GetTicketDetailAsync(ticketId, It.IsAny<CancellationToken>()))
        .ThrowsAsync(new KeyNotFoundException($"Ticket {ticketId} not found."));

    var query = new GetTicketDetailQuery(ticketId);

    // Act
    Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

    // Assert
    await act.Should().ThrowAsync<KeyNotFoundException>()
        .WithMessage($"Ticket {ticketId} not found.");
  }


  [Fact]
  public async Task Handle_Should_Propagate_Exception_If_Service_Fails()
  {
    // Arrange
    var ticketId = Guid.NewGuid();
    _ticketServiceMock
        .Setup(s => s.GetTicketDetailAsync(ticketId, It.IsAny<CancellationToken>()))
        .ThrowsAsync(new InvalidOperationException("Some error"));

    var query = new GetTicketDetailQuery(ticketId);

    // Act
    Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

    // Assert
    await act.Should().ThrowAsync<InvalidOperationException>()
        .WithMessage("Some error");
  }
}
