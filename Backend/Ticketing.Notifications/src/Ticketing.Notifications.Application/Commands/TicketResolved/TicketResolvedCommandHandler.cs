
using MediatR;
using Ticketing.Notifications.Application.Services.Interfaces;

namespace Ticketing.Notifications.Application.Commands.TicketResolved;
    public class TicketResolvedCommandHandler : IRequestHandler<TicketResolvedCommand>
{

  private readonly INotificationsService _notificationsService;

  public TicketResolvedCommandHandler(INotificationsService notificationsService)
  {
    _notificationsService = notificationsService;
  }

  public async Task Handle(TicketResolvedCommand request, CancellationToken cancellationToken)
  {
    await _notificationsService.NotifyTicketResolvedAsync(request.TicketId, request.UserId, cancellationToken);
  }

}
