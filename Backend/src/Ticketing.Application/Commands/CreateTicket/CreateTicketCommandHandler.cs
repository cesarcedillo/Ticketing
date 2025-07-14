using MediatR;
using Ticketing.Application.Dtos;
using Ticketing.Application.Services.Interfaces;

namespace Ticketing.Application.Commands.CreateTicket
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
