using MediatR;
using Ticketing.Ticket.Application.Dtos.Responses;
using Ticketing.Ticket.Application.Services.Interfaces;

namespace Ticketing.Ticket.Application.Queries.GetUserByName
{
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
}
