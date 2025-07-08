using AutoMapper;
using MediatR;
using Ticketing.Application.Dtos;
using Ticketing.Domain.Aggregates;

namespace Ticketing.Application.Queries.GetTicketById;

public class GetTicketByIdQueryHandler(IMapper _mapper)  : IRequestHandler<GetTicketByIdQuery, GetTicketDto>
{
  public Task<GetTicketDto> Handle(GetTicketByIdQuery query, CancellationToken cancellationToken)
  {
    var ticket = Ticket.Crear(Guid.NewGuid());

    var ticketDto = _mapper.Map<GetTicketDto>(ticket);

    return Task.FromResult(ticketDto);
  }
}
