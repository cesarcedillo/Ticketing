using MediatR;
using Ticketing.User.Application.Dto.Responses;

namespace Ticketing.User.Application.Queries.GetUserByName;
public sealed record GetUserByNameQuery(string UserName) : IRequest<UserResponse>;
