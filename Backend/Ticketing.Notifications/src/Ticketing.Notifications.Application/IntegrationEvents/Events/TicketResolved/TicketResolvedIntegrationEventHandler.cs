using MediatR;
using Ticketing.Core.Service.IntegrationEvents;
using Ticketing.Notifications.Application.Commands.TicketResolved;

namespace Ticketing.Notifications.Application.IntegrationEvents.Events.TicketResolved;

public class TicketResolvedIntegrationEventHandler : IIntegrationEventHandler<TicketResolvedIntegrationEvent>
{

  private readonly IMediator _mediator;

  public TicketResolvedIntegrationEventHandler(IMediator mediator)
  {
    _mediator = mediator;
  }

  public async Task Handle(TicketResolvedIntegrationEvent @event, CancellationToken cancellationToken = default)
  {
    var command = new TicketResolvedCommand(@event.TicketId, @event.UserId);

    await _mediator.Send(command, cancellationToken);
  }
}

