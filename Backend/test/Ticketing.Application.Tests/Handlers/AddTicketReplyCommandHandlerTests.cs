﻿using FluentAssertions;
using Moq;
using Ticketing.Application.Commands.AddTicketReply;
using Ticketing.Application.Services.Interfaces;

namespace Ticketing.Application.Tests.Handlers;
public class AddTicketReplyCommandHandlerTests
{
  private readonly Mock<ITicketService> _ticketServiceMock;
  private readonly AddTicketReplyCommandHandler _handler;

  public AddTicketReplyCommandHandlerTests()
  {
    _ticketServiceMock = new Mock<ITicketService>();
    _handler = new AddTicketReplyCommandHandler(_ticketServiceMock.Object);
  }

  [Fact]
  public async Task Handle_Should_Call_Service_And_Return_ReplyId()
  {
    // Arrange
    var fakeReplyId = Guid.NewGuid();
    _ticketServiceMock
        .Setup(s => s.AddReplyAsync(
            It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(fakeReplyId);

    var command = new AddTicketReplyCommand(Guid.NewGuid(), "Test reply", Guid.NewGuid());

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    result.Should().Be(fakeReplyId);
    _ticketServiceMock.Verify(s =>
        s.AddReplyAsync(command.TicketId, command.Text, command.UserId, It.IsAny<CancellationToken>()),
        Times.Once);
  }

  [Fact]
  public async Task Handle_Should_Propagate_Exception_If_Service_Fails()
  {
    // Arrange
    _ticketServiceMock
        .Setup(s => s.AddReplyAsync(
            It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
        .ThrowsAsync(new InvalidOperationException("Some error"));

    var command = new AddTicketReplyCommand(Guid.NewGuid(), "Test reply", Guid.NewGuid());

    // Act
    Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

    // Assert
    await act.Should().ThrowAsync<InvalidOperationException>()
        .WithMessage("Some error");
  }
}
