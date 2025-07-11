using MediatR;
using Ticketing.Application.Dtos.Responses;

namespace Ticketing.Application.Queries.GetTicketDetail;
public sealed record GetTicketDetailQuery : IRequest<TicketDetailResponse>
{
  public Guid TicketId { get; set; }

}
