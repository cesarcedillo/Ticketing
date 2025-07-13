namespace Ticketing.Application.Dtos.Requests;
public sealed record AddTicketReplyRequest
{
  public string Text { get; set; } = default!;
  public Guid UserId { get; set; }
}
