using MediatR;
using Ticketing.Ticket.Application.Dtos;
using Ticketing.Ticket.Application.Services.Interfaces;

namespace Ticketing.Ticket.Application.Commands.CreateTicket
{
  public class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, CreateTicketResponse>
  {
    private readonly ITicketService _ticketService;

    public CreateTicketCommandHandler(ITicketService ticketService)
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
