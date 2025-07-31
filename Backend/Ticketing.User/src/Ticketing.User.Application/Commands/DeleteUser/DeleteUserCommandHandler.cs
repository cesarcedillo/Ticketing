using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Ticketing.User.Application.Services.Interfaces;

namespace Ticketing.User.Application.Commands.DeleteUser;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
{
  private readonly IUserService _userService;

  public DeleteUserCommandHandler(IUserService userService)
  {
    _userService = userService;
  }

  public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
  {
    await _userService.DeleteUserAsync(request.UserName, cancellationToken);
    return Unit.Value;
  }
}
