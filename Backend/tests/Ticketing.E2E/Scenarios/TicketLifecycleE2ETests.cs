using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Ticketing.E2E.Fixtures;
using Ticketing.E2E.Helpers;

namespace Ticketing.E2E.Scenarios;

public class TicketLifecycleE2ETests : IClassFixture<BffApiFixture>
{
  private readonly BffApiFixture _fx;

  public TicketLifecycleE2ETests(BffApiFixture fx) => _fx = fx;

  [Fact]
  public async Task CreateTicket_ThenGetDetail_ShouldReturnTicket()
  {
    _fx.CurrentUser.Should().NotBeNull();
    Guid userId = Guid.Parse(_fx.CurrentUser!.Id!);

    var createPayload = new
    {
      subject = "E2E - subject",
      description = "E2E - description",
      userId
    };

    var createResponse = await _fx.Client.PostAsJsonAsync("/api/Ticket", createPayload);
    createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

    var ticketId = await createResponse.Content.ReadFromJsonAsync<Guid>();
    ticketId.Should().NotBe(Guid.Empty);

    var detailResponse = await _fx.Client.GetAsync($"/api/Ticket/{ticketId}");
    detailResponse.StatusCode.Should().Be(HttpStatusCode.OK);

    var detail = await detailResponse.Content.ReadFromJsonAsync<TicketDetailResponseBff>(
      new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    detail.Should().NotBeNull();
    detail!.Id.Should().Be(ticketId);
    detail.Subject.Should().Be(createPayload.subject);
    detail.Description.Should().Be(createPayload.description);
  }

  [Fact]
  public async Task AddReply_ThenGetDetail_ShouldContainReply()
  {
    _fx.CurrentUser.Should().NotBeNull();
    Guid userId = Guid.Parse(_fx.CurrentUser!.Id!);

    // 1) Create ticket
    var createResponse = await _fx.Client.PostAsJsonAsync("/api/Ticket", new
    {
      subject = "E2E - reply subject",
      description = "E2E - reply description",
      userId
    });
    createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
    var ticketId = await createResponse.Content.ReadFromJsonAsync<Guid>();

    // 2) Add reply
    var replyText = $"E2E reply {Guid.NewGuid()}";
    var replyResponse = await _fx.Client.PostAsJsonAsync($"/api/Ticket/{ticketId}/replies", new
    {
      text = replyText,
      userId
    });
    replyResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

    // 3) Get detail and verify reply exists
    var detailResponse = await _fx.Client.GetAsync($"/api/Ticket/{ticketId}");
    detailResponse.StatusCode.Should().Be(HttpStatusCode.OK);

    var detail = await detailResponse.Content.ReadFromJsonAsync<TicketDetailResponseBff>(
      new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    detail.Should().NotBeNull();
    detail!.Replies.Should().NotBeNull();
    detail.Replies!.Any(r => r.Text == replyText).Should().BeTrue();
  }

  [Fact]
  public async Task MarkAsResolved_ThenGetDetail_ShouldShowResolvedStatus()
  {
    _fx.CurrentUser.Should().NotBeNull();
    Guid userId = Guid.Parse(_fx.CurrentUser!.Id!);

    // Create ticket
    var createResponse = await _fx.Client.PostAsJsonAsync("/api/Ticket", new
    {
      subject = "E2E - resolve subject",
      description = "E2E - resolve description",
      userId
    });
    createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
    var ticketId = await createResponse.Content.ReadFromJsonAsync<Guid>();

    // Mark as resolved
    var resolveResponse = await _fx.Client.PatchAsync($"/api/Ticket/{ticketId}/mark-as-resolved", content: null);
    resolveResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

    // Verify status changed
    var detailResponse = await _fx.Client.GetAsync($"/api/Ticket/{ticketId}");
    detailResponse.StatusCode.Should().Be(HttpStatusCode.OK);

    var detail = await detailResponse.Content.ReadFromJsonAsync<TicketDetailResponseBff>(
      new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    detail.Should().NotBeNull();
    detail!.Status.Should().NotBeNullOrWhiteSpace();
    // aquí validaremos el literal exacto cuando lo sepamos ("Resolved", "RESOLVED", etc.)
  }

  [Fact]
  public async Task GetTicketDetail_Inexistent_ShouldReturn404()
  {
    var id = Guid.NewGuid();
    var response = await _fx.Client.GetAsync($"/api/Ticket/{id}");
    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
  }
}
