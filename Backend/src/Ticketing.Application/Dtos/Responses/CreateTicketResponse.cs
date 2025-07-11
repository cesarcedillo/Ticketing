namespace Ticketing.Application.Dtos;
public sealed record CreateTicketResponse
{
  public Guid TicketId { get; set; }
}
