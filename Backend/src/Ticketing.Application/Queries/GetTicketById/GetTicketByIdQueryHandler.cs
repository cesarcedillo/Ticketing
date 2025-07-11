using AutoMapper;
using MediatR;
using Ticketing.Application.Dtos;
using Ticketing.Domain.Aggregates;
using Ticketing.Domain.Entities;

namespace Ticketing.Application.Queries.GetTicketById;

public class GetTicketByIdQueryHandler(IMapper _mapper)  : IRequestHandler<GetTicketByIdQuery, CreateTicketResponse>
{
  public Task<CreateTicketResponse> Handle(GetTicketByIdQuery query, CancellationToken cancellationToken)
  {
    var user = new User(Guid.NewGuid(), "user name", [1], Domain.Enums.UserType.Customer);
    var ticket = new Ticket("subject...", "some description", user);

    var ticketDto = _mapper.Map<CreateTicketResponse>(ticket);

    return Task.FromResult(ticketDto);
  }
}
