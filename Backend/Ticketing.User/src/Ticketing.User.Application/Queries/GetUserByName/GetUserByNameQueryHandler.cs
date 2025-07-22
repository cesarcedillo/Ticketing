using MediatR;
using Ticketing.User.Application.Dto.Responses;
using Ticketing.User.Application.Services.Interfaces;

namespace Ticketing.User.Application.Queries.GetUserByName;
public class GetUserByNameQueryHandler : IRequestHandler<GetUserByNameQuery, UserResponse>
{
  private readonly IUserService _userService;

  public GetUserByNameQueryHandler(IUserService userService)
  {
    _userService = userService;
  }

  public async Task<UserResponse> Handle(GetUserByNameQuery request, CancellationToken cancellationToken)
  {
    var user = await _userService.GetUserByUserNameAsync(request.UserName, cancellationToken);

    if (user == null)
      throw new KeyNotFoundException($"User {request.UserName} not found.");

    return user;
  }
}