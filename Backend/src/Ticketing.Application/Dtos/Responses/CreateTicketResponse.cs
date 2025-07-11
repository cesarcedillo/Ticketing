namespace Ticketing.Application.Dtos;
public record CreateTicketResponse
{
  public required Guid TicketId { get; set; }
}
