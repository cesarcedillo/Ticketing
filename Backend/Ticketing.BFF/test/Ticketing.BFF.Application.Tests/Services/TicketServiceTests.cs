using AutoMapper;
using Moq;
using Ticket.Cliente.NswagAutoGen.HttpClientFactoryImplementation;
using Ticketing.BFF.Application.Dto.Responses;
using Ticketing.BFF.Application.Services;

namespace Ticketing.BFF.Application.Tests.Services;
public class TicketServiceTests
{
  private readonly Mock<ITicketClient> _ticketClientMock;
  private readonly Mock<IMapper> _mapperMock;
  private readonly TicketService _service;

  public TicketServiceTests()
  {
    _ticketClientMock = new Mock<ITicketClient>();
    _mapperMock = new Mock<IMapper>();
    _service = new TicketService(_ticketClientMock.Object, _mapperMock.Object);
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
  public async Task GetTicketDetailAsync_MapsAndReturnsDetail()
  {
    // Arrange
    var ticketId = Guid.NewGuid();
    var ticketDetail = new TicketDetailResponse();
    var expectedResponse = new TicketDetailResponseBff();

    _ticketClientMock
        .Setup(c => c.GetTicketDetailAsync(ticketId, It.IsAny<CancellationToken>()))
        .ReturnsAsync(ticketDetail);

    _mapperMock
        .Setup(m => m.Map<TicketDetailResponseBff>(ticketDetail))
        .Returns(expectedResponse);

    // Act
    var result = await _service.GetTicketDetailAsync(ticketId, CancellationToken.None);

    // Assert
    Assert.Same(expectedResponse, result);
  }

  [Fact]
  public async Task ListTicketsAsync_MapsAndReturnsList()
  {
    // Arrange
    string? status = "Open";
    Guid? userId = Guid.NewGuid();
    var tickets = new List<TicketResponse> { new TicketResponse(), new TicketResponse() };
    var expectedMapped = new List<TicketResponseBff> { new TicketResponseBff(), new TicketResponseBff() };

    _ticketClientMock
        .Setup(c => c.ListTicketsAsync(status, userId, It.IsAny<CancellationToken>()))
        .ReturnsAsync(tickets);

    _mapperMock
        .Setup(m => m.Map<IReadOnlyList<TicketResponseBff>>(tickets))
        .Returns(expectedMapped);

    // Act
    var result = await _service.ListTicketsAsync(status, userId, CancellationToken.None);

    // Assert
    Assert.Equal(expectedMapped, result);
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
}