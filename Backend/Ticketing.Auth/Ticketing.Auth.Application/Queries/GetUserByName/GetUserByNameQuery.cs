using MediatR;
using Ticketing.Auth.Application.Dtos.Responses;

namespace Ticketing.Auth.Application.Queries.GetUserByName;
public sealed record GetUserByNameQuery(string UserName) : IRequest<UserResponse>;