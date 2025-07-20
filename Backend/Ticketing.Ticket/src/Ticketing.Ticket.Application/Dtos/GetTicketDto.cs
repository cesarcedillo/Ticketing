namespace Ticketing.Ticket.Application.Dtos;
public record GetTicketDto
{
  public required Guid TicketId { get; set; }
}
