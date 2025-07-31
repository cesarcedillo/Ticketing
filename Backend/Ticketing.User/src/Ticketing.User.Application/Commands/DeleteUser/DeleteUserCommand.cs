using MediatR;

namespace Ticketing.User.Application.Commands.DeleteUser;
public record DeleteUserCommand(string UserName) : IRequest<Unit>;
