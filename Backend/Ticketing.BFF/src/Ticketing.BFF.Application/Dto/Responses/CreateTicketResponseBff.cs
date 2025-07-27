
namespace Ticketing.BFF.Application.Dto.Responses;
public sealed record CreateTicketResponseBff
{
  public Guid TicketId { get; set; }
}
