using MediatR;
using Ticketing.User.Application.Dto.Responses;
using Ticketing.User.Application.Services.Interfaces;

namespace Ticketing.User.Application.Queries.GetUsersByIds;
public class GetUsersByIdsQueryHandler : IRequestHandler<GetUsersByIdsQuery, List<UserResponse>>
{
  private readonly IUserService _userService;

  public GetUsersByIdsQueryHandler(IUserService userService)
  {
    _userService = userService;
  }

  public async Task<List<UserResponse>> Handle(GetUsersByIdsQuery request, CancellationToken cancellationToken)
  {
    var users = await _userService.GetUsersByIdsAsync(request.UserIds, cancellationToken);
    return users.ToList();
  }
}
