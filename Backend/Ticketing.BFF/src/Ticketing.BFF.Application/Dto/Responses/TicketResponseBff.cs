namespace Ticketing.BFF.Application.Dto.Responses;
public sealed record TicketResponseBff
{
  public Guid Id { get; set; }
  public string Subject { get; set; } = string.Empty;
  public string Status { get; set; } = string.Empty;
  public string UserName { get; set; } = string.Empty;
  public Guid UserId { get; set; }
  public string Avatar { get; set; } = string.Empty;
}