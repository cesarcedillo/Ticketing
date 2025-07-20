namespace Ticketing.Application.Dtos.Responses;
public sealed record TicketReplyResponse
{
  public Guid Id { get; set; }
  public string Text { get; set; } = string.Empty;
  public DateTime CreatedAt { get; set; }
  public Guid UserId { get; set; }
  public string UserName { get; set; } = string.Empty;
  public string Avatar { get; set; } = string.Empty;
}
