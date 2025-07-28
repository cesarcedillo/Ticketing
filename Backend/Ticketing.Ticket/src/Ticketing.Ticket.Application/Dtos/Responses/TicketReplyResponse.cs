namespace Ticketing.Ticket.Application.Dtos.Responses;
public sealed record TicketReplyResponse
{
  public Guid Id { get; set; }
  public string Text { get; set; } = string.Empty;
  public DateTime CreatedAt { get; set; }
  public Guid UserId { get; set; }
}
