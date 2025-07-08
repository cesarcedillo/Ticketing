using MediatR;
using Ticketing.Application.Dtos;

namespace Ticketing.Application.Queries.GetTicketById;
public sealed record GetTicketByIdQuery : IRequest<GetTicketDto>, IBaseRequest, IEquatable<GetTicketByIdQuery>
{
  public required Guid TicketId { get; set; }
}
