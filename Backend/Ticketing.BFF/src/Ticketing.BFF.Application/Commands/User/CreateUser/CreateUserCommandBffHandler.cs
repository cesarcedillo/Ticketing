using MediatR;
using Ticketing.BFF.Application.Dto.Responses;
using Ticketing.BFF.Application.Services.Interfaces;

namespace Ticketing.BFF.Application.Commands.User.CreateUser;
public class CreateUserCommandBffHandler : IRequestHandler<CreateUserCommandBff, UserResponseBff>
{
  private readonly IUserService _userService;

  public CreateUserCommandBffHandler(IUserService userService)
  {
    _userService = userService;
  }

  public async Task<UserResponseBff> Handle(CreateUserCommandBff command, CancellationToken cancellationToken)
  {
    return await _userService.CreateUserAsync(command.UserName, command.Password, command.Avatar, command.Role, cancellationToken);    
  }
}