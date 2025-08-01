using MediatR;
using Ticketing.BFF.Application.Dto.Responses;

namespace Ticketing.BFF.Application.Commands.User.CreateUser;
public sealed record CreateUserCommandBff(string UserName, string Password, string Avatar, string Role) : IRequest<UserResponseBff>;
