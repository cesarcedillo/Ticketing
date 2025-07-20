using MediatR;
using Ticketing.Auth.Application.Dtos.Responses;
using Ticketing.Auth.Application.Services.Interfaces;

namespace Ticketing.Auth.Application.Queries.GetUserByName;
public class GetUserByNameQueryHandler : IRequestHandler<GetUserByNameQuery, UserResponse>
{
  private readonly IUserService _userService;

  public GetUserByNameQueryHandler(IUserService userService)
  {
    _userService = userService;
  }

  public async Task<UserResponse> Handle(GetUserByNameQuery request, CancellationToken cancellationToken)
  {
    return await _userService.GetUserByUserNameAsync(request.UserName, cancellationToken);
  }

}
