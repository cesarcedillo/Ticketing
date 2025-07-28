namespace Ticketing.BFF.Application.Dto.Responses;
public class TicketReplyResponseBff
{
  public Guid Id { get; set; }
  public string Text { get; set; } = string.Empty;
  public DateTime CreatedAt { get; set; }
  public UserResponseBff User { get; set; }
}

