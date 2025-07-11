using MediatR;
using Ticketing.Application.Dtos;

namespace Ticketing.Application.Queries.GetTicketById;
public sealed record GetTicketByIdQuery : IRequest<CreateTicketResponse>, IBaseRequest, IEquatable<GetTicketByIdQuery>
{
  public required Guid TicketId { get; set; }
}
