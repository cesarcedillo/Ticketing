using MediatR;
using Ticketing.User.Application.Dto.Responses;

namespace Ticketing.User.Application.Commands.CreateUser;
public sealed record CreateUserCommand(string UserName, string Avatar, string Role) : IRequest<UserResponse>;