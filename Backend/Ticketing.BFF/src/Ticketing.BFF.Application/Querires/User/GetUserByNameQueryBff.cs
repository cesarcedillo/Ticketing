using MediatR;
using Ticketing.BFF.Application.Dto.Responses;

namespace Ticketing.BFF.Application.Querires.User;
public sealed record GetUserByNameQueryBff(string UserName) : IRequest<UserResponseBff>;