using FluentAssertions;
using Moq;
using Ticketing.Application.Dtos.Responses;
using Ticketing.Application.Queries.ListTickets;
using Ticketing.Application.Services.Interfaces;

namespace Ticketing.Application.Tests.Handlers;
  public class ListTicketsQueryHandlerTests
  {
    private readonly Mock<ITicketService> _ticketServiceMock;
    private readonly ListTicketsQueryHandler _handler;

    public ListTicketsQueryHandlerTests()
    {
      _ticketServiceMock = new Mock<ITicketService>();
      _handler = new ListTicketsQueryHandler(_ticketServiceMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_Ticket_List_From_Service()
    {
      // Arrange
      var expectedList = new List<TicketResponse>
            {
                new TicketResponse { Id = Guid.NewGuid(), Subject = "A", Status = "Open" },
                new TicketResponse { Id = Guid.NewGuid(), Subject = "B", Status = "Resolved" }
            };

      _ticketServiceMock
          .Setup(s => s.ListTicketsAsync(It.IsAny<string?>(), It.IsAny<Guid?>(), It.IsAny<CancellationToken>()))
          .ReturnsAsync(expectedList);

      var query = new ListTicketsQuery(null, null);

      // Act
      var result = await _handler.Handle(query, CancellationToken.None);

      // Assert
      result.Should().BeEquivalentTo(expectedList);
      _ticketServiceMock.Verify(s =>
          s.ListTicketsAsync(query.Status, query.UserId, It.IsAny<CancellationToken>()),
          Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Return_Empty_List_When_Service_Returns_Empty()
    {
      // Arrange
      var expectedList = new List<TicketResponse>();

      _ticketServiceMock
          .Setup(s => s.ListTicketsAsync(It.IsAny<string?>(), It.IsAny<Guid?>(), It.IsAny<CancellationToken>()))
          .ReturnsAsync(expectedList);

      var query = new ListTicketsQuery(null, null);

      // Act
      var result = await _handler.Handle(query, CancellationToken.None);

      // Assert
      result.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_Should_Propagate_Exception_If_Service_Fails()
    {
      // Arrange
      _ticketServiceMock
          .Setup(s => s.ListTicketsAsync(It.IsAny<string?>(), It.IsAny<Guid?>(), It.IsAny<CancellationToken>()))
          .ThrowsAsync(new InvalidOperationException("Some error"));

      var query = new ListTicketsQuery("Open", Guid.NewGuid());

      // Act
      Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

      // Assert
      await act.Should().ThrowAsync<InvalidOperationException>()
          .WithMessage("Some error");
    }
  }

