using AutoMapper;
using Moq;
using Ticket.Cliente.NswagAutoGen.HttpClientFactoryImplementation;
using Ticketing.BFF.Application.Dto.Responses;
using Ticketing.BFF.Application.Services;
using User.Cliente.NswagAutoGen.HttpClientFactoryImplementation;
using FluentAssertions;

namespace Ticketing.BFF.Application.Tests.Services;
public class TicketServiceTests
{
  private readonly Mock<ITicketClient> _ticketClientMock;
  private readonly Mock<IUserClient> _userClientMock;
  private readonly Mock<IMapper> _mapperMock;
  private readonly TicketService _service;

  public TicketServiceTests()
  {
    _ticketClientMock = new Mock<ITicketClient>();
    _userClientMock = new Mock<IUserClient>();
    _mapperMock = new Mock<IMapper>();
    _service = new TicketService(_ticketClientMock.Object, _userClientMock.Object, _mapperMock.Object);
  }

  [Fact]
  public async Task AddReplyAsync_CallsClientWithCorrectRequest()
  {
    // Arrange
    var ticketId = Guid.NewGuid();
    var userId = Guid.NewGuid();
    var text = "Comentario de prueba";
    AddTicketReplyRequest capturedRequest = null;

    _ticketClientMock
        .Setup(c => c.AddTicketReplyAsync(ticketId, It.IsAny<AddTicketReplyRequest>(), It.IsAny<CancellationToken>()))
        .Callback<Guid, AddTicketReplyRequest, CancellationToken>((id, req, token) => capturedRequest = req)
        .Returns(Task.CompletedTask);

    // Act
    await _service.AddReplyAsync(ticketId, text, userId, CancellationToken.None);

    // Assert
    _ticketClientMock.Verify(c =>
        c.AddTicketReplyAsync(ticketId, It.IsAny<AddTicketReplyRequest>(), It.IsAny<CancellationToken>()), Times.Once);

    Assert.NotNull(capturedRequest);
    Assert.Equal(text, capturedRequest.Text);
    Assert.Equal(userId, capturedRequest.UserId);
  }

  [Fact]
  public async Task CreateTicketAsync_CallsClientAndReturnsGuid()
  {
    // Arrange
    var subject = "Test Subject";
    var description = "Test Description";
    var userId = Guid.NewGuid();
    var expectedId = Guid.NewGuid();
    CreateTicketRequest capturedRequest = null;

    _ticketClientMock
        .Setup(c => c.CreateTicketAsync(It.IsAny<CreateTicketRequest>(), It.IsAny<CancellationToken>()))
        .Callback<CreateTicketRequest, CancellationToken>((req, token) => capturedRequest = req)
        .ReturnsAsync(expectedId);

    // Act
    var result = await _service.CreateTicketAsync(subject, description, userId, CancellationToken.None);

    // Assert
    Assert.Equal(expectedId, result);
    _ticketClientMock.Verify(c =>
        c.CreateTicketAsync(It.IsAny<CreateTicketRequest>(), It.IsAny<CancellationToken>()), Times.Once);

    Assert.NotNull(capturedRequest);
    Assert.Equal(subject, capturedRequest.Subject);
    Assert.Equal(description, capturedRequest.Description);
    Assert.Equal(userId, capturedRequest.UserId);
  }

  [Fact]
  public async Task MarkTicketAsResolvedAsync_CallsClient()
  {
    // Arrange
    var ticketId = Guid.NewGuid();

    _ticketClientMock
        .Setup(c => c.MarkAsResolvedAsync(ticketId, It.IsAny<CancellationToken>()))
        .Returns(Task.CompletedTask);

    // Act
    await _service.MarkTicketAsResolvedAsync(ticketId, CancellationToken.None);

    // Assert
    _ticketClientMock.Verify(c =>
        c.MarkAsResolvedAsync(ticketId, It.IsAny<CancellationToken>()), Times.Once);
  }

  [Fact]
  public async Task GetTicketDetailAsync_MapsUsersAndRepliesCorrectly()
  {
    // Arrange
    var ticketId = Guid.NewGuid();
    var userId = Guid.NewGuid();
    var replyUserId = Guid.NewGuid();
    var ticketReplyId = Guid.NewGuid();

    var ticketDetail = new TicketDetailResponse
    {
      UserId = userId,
      Replies = new List<TicketReplyResponse>
        {
            new TicketReplyResponse { Id = ticketReplyId, UserId = replyUserId }
        }
    };

    var users = new List<UserResponse>
    {
        new UserResponse { Id = userId, UserName = "ticketOwner" },
        new UserResponse { Id = replyUserId, UserName = "replier" }
    };

    // Mocks de mapeo
    var mappedTicket = new TicketDetailResponseBff
    {
      Replies = new List<TicketReplyResponseBff>
        {
            new TicketReplyResponseBff { Id = ticketReplyId }
        }
    };
    var mappedTicketOwner = new UserResponseBff { Id = userId.ToString(), UserName = "ticketOwner" };
    var mappedReplyUser = new UserResponseBff { Id = replyUserId.ToString(), UserName = "replier" };

    _ticketClientMock
        .Setup(c => c.GetTicketDetailAsync(ticketId, It.IsAny<CancellationToken>()))
        .ReturnsAsync(ticketDetail);

    _userClientMock
        .Setup(c => c.GetUsersByIdsAsync(It.Is<HashSet<Guid>>(set => set.SetEquals(new[] { userId, replyUserId })), It.IsAny<CancellationToken>()))
        .ReturnsAsync(users);

    _mapperMock
        .Setup(m => m.Map<TicketDetailResponseBff>(ticketDetail))
        .Returns(mappedTicket);

    _mapperMock
        .Setup(m => m.Map<UserResponseBff>(users[0]))
        .Returns(mappedTicketOwner);
    _mapperMock
        .Setup(m => m.Map<UserResponseBff>(users[1]))
        .Returns(mappedReplyUser);

    // Act
    var result = await _service.GetTicketDetailAsync(ticketId, CancellationToken.None);

    // Assert
    Assert.Same(mappedTicket, result);
    Assert.Same(mappedTicketOwner, result.User);
    Assert.Same(mappedReplyUser, result.Replies[0].User);

    _ticketClientMock.Verify(c => c.GetTicketDetailAsync(ticketId, It.IsAny<CancellationToken>()), Times.Once);
    _userClientMock.Verify(c => c.GetUsersByIdsAsync(It.IsAny<HashSet<Guid>>(), It.IsAny<CancellationToken>()), Times.Once);
    _mapperMock.Verify(m => m.Map<TicketDetailResponseBff>(ticketDetail), Times.Once);
    _mapperMock.Verify(m => m.Map<UserResponseBff>(It.IsAny<UserResponse>()), Times.Exactly(2));
  }


  [Fact]
  public async Task ListTicketsAsync_MapsTicketsAndUsers_Correctly()
  {
    // Arrange
    string? status = "Open";
    Guid? userId = Guid.NewGuid();

    var ticket1Id = Guid.NewGuid();
    var ticket2Id = Guid.NewGuid();
    var ticket1UserId = Guid.NewGuid();
    var ticket2UserId = Guid.NewGuid();

    var tickets = new List<TicketResponse>
    {
        new TicketResponse { Id = ticket1Id, UserId = ticket1UserId, Subject = "T1" },
        new TicketResponse { Id = ticket2Id, UserId = ticket2UserId, Subject = "T2" }
    };

    var users = new List<UserResponse>
    {
        new UserResponse { Id = ticket1UserId, UserName = "user1" },
        new UserResponse { Id = ticket2UserId, UserName = "user2" }
    };

    var ticketBff1 = new TicketResponseBff { Id = ticket1Id, Subject = "T1" };
    var ticketBff2 = new TicketResponseBff { Id = ticket2Id, Subject = "T2" };
    var ticketBffs = new List<TicketResponseBff> { ticketBff1, ticketBff2 };

    var userBff1 = new UserResponseBff { Id = ticket1UserId.ToString(), UserName = "user1" };
    var userBff2 = new UserResponseBff { Id = ticket2UserId.ToString(), UserName = "user2" };

    _ticketClientMock
        .Setup(c => c.ListTicketsAsync(status, userId, It.IsAny<CancellationToken>()))
        .ReturnsAsync(tickets);

    _userClientMock
        .Setup(c => c.GetUsersByIdsAsync(It.Is<HashSet<Guid>>(set =>
            set.SetEquals(new[] { ticket1UserId, ticket2UserId })), It.IsAny<CancellationToken>()))
        .ReturnsAsync(users);

    _mapperMock
        .Setup(m => m.Map<IReadOnlyList<TicketResponseBff>>(tickets))
        .Returns(ticketBffs);

    _mapperMock
        .Setup(m => m.Map<UserResponseBff>(users[0]))
        .Returns(userBff1);

    _mapperMock
        .Setup(m => m.Map<UserResponseBff>(users[1]))
        .Returns(userBff2);

    // Act
    var result = await _service.ListTicketsAsync(status, userId, CancellationToken.None);

    // Assert
    result.Should().HaveCount(2);
    result.First(r => r.Id == ticket1Id).User.Should().Be(userBff1);
    result.First(r => r.Id == ticket2Id).User.Should().Be(userBff2);

    _ticketClientMock.Verify(c => c.ListTicketsAsync(status, userId, It.IsAny<CancellationToken>()), Times.Once);
    _userClientMock.Verify(c => c.GetUsersByIdsAsync(It.IsAny<HashSet<Guid>>(), It.IsAny<CancellationToken>()), Times.Once);
    _mapperMock.Verify(m => m.Map<IReadOnlyList<TicketResponseBff>>(tickets), Times.Once);
    _mapperMock.Verify(m => m.Map<UserResponseBff>(users[0]), Times.Once);
    _mapperMock.Verify(m => m.Map<UserResponseBff>(users[1]), Times.Once);
  }

  [Fact]
  public async Task ListTicketsAsync_EmptyList_ReturnsEmpty()
  {
    // Arrange
    string? status = null;
    Guid? userId = null;
    var tickets = new List<TicketResponse>();
    var ticketBffs = new List<TicketResponseBff>();
    var users = new List<UserResponse>();

    _ticketClientMock
        .Setup(c => c.ListTicketsAsync(status, userId, It.IsAny<CancellationToken>()))
        .ReturnsAsync(tickets);

    _userClientMock
        .Setup(c => c.GetUsersByIdsAsync(It.IsAny<HashSet<Guid>>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(users);

    _mapperMock
        .Setup(m => m.Map<IReadOnlyList<TicketResponseBff>>(tickets))
        .Returns(ticketBffs);

    // Act
    var result = await _service.ListTicketsAsync(status, userId, CancellationToken.None);

    // Assert
    result.Should().BeEmpty();
    _ticketClientMock.Verify(c => c.ListTicketsAsync(status, userId, It.IsAny<CancellationToken>()), Times.Once);
    _userClientMock.Verify(c => c.GetUsersByIdsAsync(It.IsAny<HashSet<Guid>>(), It.IsAny<CancellationToken>()), Times.Once);
    _mapperMock.Verify(m => m.Map<IReadOnlyList<TicketResponseBff>>(tickets), Times.Once);
  }


}