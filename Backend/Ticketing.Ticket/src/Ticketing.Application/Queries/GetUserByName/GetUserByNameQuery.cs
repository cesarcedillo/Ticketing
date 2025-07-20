using MediatR;
using Ticketing.Application.Dtos.Responses;

namespace Ticketing.Application.Queries.GetUserByName;
public sealed record GetUserByNameQuery(string UserName)
    : IRequest<UserResponse>;
