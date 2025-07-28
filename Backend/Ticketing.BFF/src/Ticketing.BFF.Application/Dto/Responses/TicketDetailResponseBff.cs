using Ticket.Cliente.NswagAutoGen.HttpClientFactoryImplementation;

namespace Ticketing.BFF.Application.Dto.Responses;
public sealed record TicketDetailResponseBff
{
  public Guid Id { get; set; }
  public string Subject { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public string Status { get; set; } = string.Empty;
  public UserResponseBff User { get; set; }
  public List<TicketReplyResponseBff> Replies { get; set; } = new();
}