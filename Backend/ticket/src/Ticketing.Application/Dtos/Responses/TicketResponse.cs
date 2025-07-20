namespace Ticketing.Application.Dtos.Responses;
public sealed record TicketResponse
{
  public Guid Id { get; set; }
  public string Subject { get; set; } = string.Empty;
  public string Status { get; set; } = string.Empty;
  public string UserName { get; set; } = string.Empty;
  public Guid UserId { get; set; }
  public string Avatar { get; set; } = string.Empty;
}

