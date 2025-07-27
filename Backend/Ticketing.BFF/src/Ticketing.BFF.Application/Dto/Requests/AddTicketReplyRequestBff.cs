namespace Ticketing.BFF.Application.Dto.Requests;
public sealed record AddTicketReplyRequestBff
{
  public string Text { get; set; } = default!;
  public Guid UserId { get; set; }
}
