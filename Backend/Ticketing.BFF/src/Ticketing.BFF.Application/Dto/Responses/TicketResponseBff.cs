namespace Ticketing.BFF.Application.Dto.Responses;
public sealed record TicketResponseBff
{
  public Guid Id { get; set; }
  public string Subject { get; set; } = string.Empty;
  public string Status { get; set; } = string.Empty;
  public UserResponseBff User { get; set; }
}