using MediatR;
using Ticketing.BFF.Application.Dto.Responses;
using Ticketing.BFF.Application.Services.Interfaces;

namespace Ticketing.BFF.Application.Querires.Ticket.GetTicketDetail;
public class GetTicketDetailQueryBffHandler : IRequestHandler<GetTicketDetailQueryBff, TicketDetailResponseBff>
{
  private readonly ITicketService _ticketService;

  public GetTicketDetailQueryBffHandler(ITicketService ticketService)
  {
    _ticketService = ticketService;
  }

  public async Task<TicketDetailResponseBff> Handle(GetTicketDetailQueryBff request, CancellationToken cancellationToken)
  {
    var ticket = await _ticketService.GetTicketDetailAsync(request.TicketId, cancellationToken);

    return ticket;
  }
}

