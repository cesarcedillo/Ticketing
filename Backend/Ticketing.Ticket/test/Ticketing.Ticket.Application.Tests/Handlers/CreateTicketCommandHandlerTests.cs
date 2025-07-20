using FluentAssertions;
using Moq;
using Ticketing.Ticket.Application.Commands.CreateTicket;
using Ticketing.Ticket.Application.Services.Interfaces;

namespace Ticketing.Ticket.Application.Tests.Handlers
{
  public class CreateTicketCommandHandlerTests
  {
    private readonly Mock<ITicketService> _ticketServiceMock;
    private readonly CreateTicketCommandHandler _handler;

    public CreateTicketCommandHandlerTests()
    {
      _ticketServiceMock = new Mock<ITicketService>();
      _handler = new CreateTicketCommandHandler(_ticketServiceMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Call_Service_And_Return_TicketId()
    {
      // Arrange
      var fakeTicketId = Guid.NewGuid();
      _ticketServiceMock
          .Setup(s => s.CreateTicketAsync(
              It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
          .ReturnsAsync(fakeTicketId);

      var command = new CreateTicketCommand("Subject", "Description", Guid.NewGuid());

      // Act
      var result = await _handler.Handle(command, CancellationToken.None);

      // Assert
      result.TicketId.Should().Be(fakeTicketId);
      _ticketServiceMock.Verify(s =>
          s.CreateTicketAsync(command.Subject, command.Description, command.UserId, It.IsAny<CancellationToken>()),
          Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Propagate_Exception_If_Service_Fails()
    {
      // Arrange
      _ticketServiceMock
          .Setup(s => s.CreateTicketAsync(
              It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
          .ThrowsAsync(new InvalidOperationException("Some error"));

      var command = new CreateTicketCommand("Subject", "Description", Guid.NewGuid());

      // Act
      Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

      // Assert
      await act.Should().ThrowAsync<InvalidOperationException>()
          .WithMessage("Some error");
    }
  }
}
