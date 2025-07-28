using MediatR;
using Ticketing.User.Application.Dto.Responses;
using Ticketing.User.Application.Services.Interfaces;

namespace Ticketing.User.Application.Commands.CreateUser;
public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserResponse>
{
  private readonly IUserService _userService;

  public CreateUserCommandHandler(IUserService userService)
  {
    _userService = userService;
  }

  public async Task<UserResponse> Handle(CreateUserCommand command, CancellationToken cancellationToken)
  {
    return await _userService.CreateUserAsync(command.UserName, command.Avatar, command.Role, cancellationToken);
  }
}