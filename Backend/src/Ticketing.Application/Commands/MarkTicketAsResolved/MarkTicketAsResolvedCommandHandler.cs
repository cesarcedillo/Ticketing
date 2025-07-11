﻿using MediatR;
using Ticketing.Application.Services.Interfaces;

namespace Ticketing.Application.Commands.MarkTicketAsResolved;
public class MarkTicketAsResolvedCommandHandler : IRequestHandler<MarkTicketAsResolvedCommand>
{
  private readonly ITicketService _ticketService;

  public MarkTicketAsResolvedCommandHandler(ITicketService ticketService)
  {
    _ticketService = ticketService;
  }

  public async Task Handle(MarkTicketAsResolvedCommand request, CancellationToken cancellationToken)
  {
    await _ticketService.MarkTicketAsResolvedAsync(request.TicketId, cancellationToken);
  }
}
