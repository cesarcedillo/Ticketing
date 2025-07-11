using MediatR;
using Ticketing.Application.Dtos.Responses;

namespace Ticketing.Application.Queries.ListTickets;
public sealed record ListTicketsQuery : IRequest<IReadOnlyList<TicketResponse>>
{
  public string? Status { get; set; }
  public Guid? UserId { get; set; }
}

