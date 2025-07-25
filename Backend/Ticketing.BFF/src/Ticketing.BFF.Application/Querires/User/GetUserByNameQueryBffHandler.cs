using MediatR;
using Ticketing.BFF.Application.Dto.Responses;
using Ticketing.BFF.Application.Services.Interfaces;

namespace Ticketing.BFF.Application.Querires.User;
public class GetUserByNameQueryBffHandler : IRequestHandler<GetUserByNameQueryBff, UserResponseBff>
{
  private readonly IUserService _userService;

  public GetUserByNameQueryBffHandler(IUserService userService)
  {
    _userService = userService;
  }

  public async Task<UserResponseBff> Handle(GetUserByNameQueryBff request, CancellationToken cancellationToken)
  {
    var user = await _userService.GetUserByUserNameAsync(request.UserName, cancellationToken);

    return user;
  }
}