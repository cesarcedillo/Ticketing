namespace Ticketing.Application.Dtos.Requests;
public sealed record CreateTicketRequest
{
  public string Subject { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public Guid UserId { get; set; }
}

