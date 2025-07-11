using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Application.Dtos;
using Ticketing.Application.Services.Interfaces;

namespace Ticketing.Application.Commands.CreateTicket
{
  public class CrearteTicketCommandHandler : IRequestHandler<CreateTicketCommand, CreateTicketResponse>
  {
    private readonly ITicketService _ticketService;

    public CrearteTicketCommandHandler(ITicketService ticketService)
    {
      _ticketService = ticketService;
    }

    public async Task<CreateTicketResponse> Handle(CreateTicketCommand command, CancellationToken cancellationToken)
    {
      var ticketId = await _ticketService.CreateTicketAsync(command.Subject,
                                                                command.Description,
                                                                command.UserId,
                                                                cancellationToken);
      return new CreateTicketResponse() { TicketId = ticketId };
    }
  }
}
