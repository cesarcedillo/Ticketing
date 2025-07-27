using Ticket.Cliente.NswagAutoGen.HttpClientFactoryImplementation;

namespace Ticketing.BFF.Application.Dto.Responses;
public sealed record TicketDetailResponseBff
{
  public Guid Id { get; set; }
  public string Subject { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public string Status { get; set; } = string.Empty;
  public Guid UserId { get; set; }
  public string UserName { get; set; } = string.Empty;
  public string Avatar { get; set; } = string.Empty;
  public List<TicketReplyResponse> Replies { get; set; } = new();
}