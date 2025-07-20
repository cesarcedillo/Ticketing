using MediatR;
using Ticketing.Ticket.Application.Dtos.Responses;

namespace Ticketing.Ticket.Application.Queries.GetUserByName;
public sealed record GetUserByNameQuery(string UserName)
    : IRequest<UserResponse>;
