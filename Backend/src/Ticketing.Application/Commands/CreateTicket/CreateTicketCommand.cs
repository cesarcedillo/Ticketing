using MediatR;
using Ticketing.Application.Dtos;

namespace Ticketing.Application.Commands.CreateTicket;
public sealed record CreateTicketCommand : IRequest<CreateTicketResponse>
{
  public string Subject { get; } = string.Empty;
  public string Description { get; } = string.Empty;
  public Guid UserId { get; }
}

