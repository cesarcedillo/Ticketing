namespace Ticketing.BFF.Application.Dto.Requests;
public sealed record CreateTicketRequestBff
{
  public string Subject { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public Guid UserId { get; set; }
}