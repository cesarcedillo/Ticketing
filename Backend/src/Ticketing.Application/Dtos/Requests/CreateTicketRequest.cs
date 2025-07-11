namespace Ticketing.Application.Dtos.Requests;
public class CreateTicketRequest
{
  public string Subject { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public Guid UserId { get; set; }
}

