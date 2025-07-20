using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MockQueryable.Moq;
using AutoMapper;
using FluentAssertions;
using MockQueryable;
using Moq;
using Ticketing.Application.Dtos.Responses;
using Ticketing.Application.Services;
using Ticketing.Core.Domain.SeedWork.Interfaces;
using Ticketing.Domain.Aggregates;
using Ticketing.Domain.Entities;
using Ticketing.Domain.Enums;
using Ticketing.Domain.Interfaces.Repositories;
using Ticketing.TestCommon.Builders;
using Ticketing.TestCommon.Fixtures;
using Xunit;

namespace Ticketing.Application.Tests.Services
{
  public class TicketServiceTests
  {
    private readonly Mock<ITicketRepository> _ticketRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly IMapper _mapper;
    private readonly TicketService _service;
    private readonly DomainFixture _fixture = new();

    public TicketServiceTests()
    {
      _ticketRepositoryMock = new Mock<ITicketRepository>();
      _userRepositoryMock = new Mock<IUserRepository>();
      _unitOfWorkMock = new Mock<IUnitOfWork>();

      var config = new MapperConfiguration(cfg =>
      {
        cfg.AddProfile<MappingProfile>();
      });
      _mapper = config.CreateMapper();

      _ticketRepositoryMock.SetupGet(r => r.UnitOfWork).Returns(_unitOfWorkMock.Object);

      _service = new TicketService(
          _ticketRepositoryMock.Object,
          _userRepositoryMock.Object,
          _mapper
      );
    }

    [Fact]
    public async Task CreateTicketAsync_Should_Create_And_Return_Id()
    {
      // Arrange
      var user = _fixture.CreateDefaultCustomer("testuser");
      _userRepositoryMock.Setup(r => r.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()))
          .ReturnsAsync(user);

      Ticket? createdTicket = null;
      _ticketRepositoryMock.Setup(r => r.Add(It.IsAny<Ticket>()))
          .Callback<Ticket>(t => createdTicket = t);

      _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
          .ReturnsAsync(1);

      // Act
      var id = await _service.CreateTicketAsync("Subject", "Description", user.Id, CancellationToken.None);

      // Assert
      id.Should().NotBeEmpty();
      createdTicket.Should().NotBeNull();
      createdTicket!.Subject.Should().Be("Subject");
      createdTicket.User.Should().Be(user);
    }

    [Fact]
    public async Task ListTicketsAsync_Should_Return_Mapped_List()
    {
      // Arrange
      var ticket = _fixture.CreateTicketWithReplies();
      var tickets = new List<Ticket> { ticket };
      var mockQueryable = tickets.AsQueryable().BuildMock();

      _ticketRepositoryMock.Setup(r => r.Query())
            .Returns(mockQueryable);

      _ticketRepositoryMock.Setup(r => r.GetUnresolvedTicketsAsync(It.IsAny<CancellationToken>()))
          .ReturnsAsync(tickets);

      var result = await _service.ListTicketsAsync(null, null, CancellationToken.None);

      // Assert
      result.Should().NotBeNullOrEmpty();
      result!.First().Subject.Should().Be("Default Subject");
    }

    [Fact]
    public async Task GetTicketDetailAsync_Should_Return_Dto_If_Exists()
    {
      // Arrange
      var ticket = _fixture.CreateTicketWithReplies();
      var tickets = new List<Ticket> { ticket };
      var mockQueryable = tickets.AsQueryable().BuildMock();

      _ticketRepositoryMock.Setup(r => r.Query())
          .Returns(mockQueryable);

      // Act
      var result = await _service.GetTicketDetailAsync(ticket.Id, CancellationToken.None);

      // Assert
      result.Should().NotBeNull();
      result!.Subject.Should().Be(ticket.Subject);
    }

    [Fact]
    public async Task GetTicketDetailAsync_Should_Return_Null_If_Not_Found()
    {
      // Arrange
      var ticketId = Guid.NewGuid();
      var tickets = new List<Ticket> {  };
      var mockQueryable = tickets.AsQueryable().BuildMock();

      _ticketRepositoryMock.Setup(r => r.Query())
          .Returns(mockQueryable);

      // Act
      var result = await _service.GetTicketDetailAsync(ticketId, CancellationToken.None);

      // Assert
      result.Should().BeNull();
    }

    [Fact]
    public async Task MarkTicketAsResolvedAsync_Should_Change_Status_And_Save()
    {
      // Arrange
      var ticket = _fixture.CreateTicketWithReplies();
      var tickets = new List<Ticket> { ticket };
      var mockQueryable = tickets.AsQueryable().BuildMock();

      _ticketRepositoryMock.Setup(r => r.Query())
          .Returns(mockQueryable);

      _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
          .ReturnsAsync(1);

      // Act
      await _service.MarkTicketAsResolvedAsync(ticket.Id, CancellationToken.None);

      // Assert
      ticket.Status.Should().Be(TicketStatus.Resolved);
    }



    [Fact]
    public async Task AddReplyAsync_Should_Add_Reply_And_Save_It()
    {
      // Arrange
      var ticket = new TicketBuilder().Build();
      var tickets = new List<Ticket> { ticket };
      var mockQueryable = tickets.AsQueryable().BuildMock();

      var agent = _fixture.CreateDefaultAgent("agent1");

      _ticketRepositoryMock.Setup(r => r.Query())
          .Returns(mockQueryable);
      _userRepositoryMock.Setup(r => r.GetByIdAsync(agent.Id, It.IsAny<CancellationToken>()))
          .ReturnsAsync(agent);

      _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
          .ReturnsAsync(1);

      // Act
      await _service.AddReplyAsync(ticket.Id, "Response", agent.Id, CancellationToken.None);

      // Assert
      _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
  }
}
