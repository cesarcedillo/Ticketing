using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.BFF.Application.Dto.Responses;
using Ticketing.BFF.Application.Services.Interfaces;

namespace Ticketing.BFF.Application.Commands.Ticket.CreateTicket;
public class CreateTicketCommandBffHandler : IRequestHandler<CreateTicketCommandBff, CreateTicketResponseBff>
{
  private readonly ITicketService _ticketService;

  public CreateTicketCommandBffHandler(ITicketService ticketService)
  {
    _ticketService = ticketService;
  }

  public async Task<CreateTicketResponseBff> Handle(CreateTicketCommandBff command, CancellationToken cancellationToken)
  {
    var ticketId = await _ticketService.CreateTicketAsync(command.Subject,
                                                              command.Description,
                                                              command.UserId,
                                                              cancellationToken);
    return new CreateTicketResponseBff() { TicketId = ticketId };
  }
}