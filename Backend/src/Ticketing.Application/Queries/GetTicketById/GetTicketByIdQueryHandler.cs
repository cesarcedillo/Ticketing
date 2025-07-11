using AutoMapper;
using MediatR;
using Ticketing.Application.Dtos;
using Ticketing.Domain.Aggregates;
using Ticketing.Domain.Entities;

namespace Ticketing.Application.Queries.GetTicketById;

public class GetTicketByIdQueryHandler(IMapper _mapper)  : IRequestHandler<GetTicketByIdQuery, GetTicketDto>
{
  public Task<GetTicketDto> Handle(GetTicketByIdQuery query, CancellationToken cancellationToken)
  {
    var user = new User(Guid.NewGuid(), "user name", [1], Domain.Enums.UserType.Customer);
    var ticket = new Ticket(Guid.NewGuid(), "subject...", "some description", user);

    var ticketDto = _mapper.Map<GetTicketDto>(ticket);

    return Task.FromResult(ticketDto);
  }
}
