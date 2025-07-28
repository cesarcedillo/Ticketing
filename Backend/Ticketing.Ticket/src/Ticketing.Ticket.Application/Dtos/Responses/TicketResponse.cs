namespace Ticketing.Ticket.Application.Dtos.Responses;
public sealed record TicketResponse
{
  public Guid Id { get; set; }
  public string Subject { get; set; } = string.Empty;
  public string Status { get; set; } = string.Empty;
  public Guid UserId { get; set; }
}

