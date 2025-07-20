using Ticketing.Auth.Application.Dtos.Responses;
using Ticketing.Auth.Application.Services.Interfaces;
using Ticketing.Auth.Domain.Interfaces;

namespace Ticketing.Auth.Application.Services;

public class UserService : IUserService
{

  private readonly IUserRepository _userRepository;

  public UserService(IUserRepository userRepository)
  {
    _userRepository = userRepository;
  }

  public async Task<UserResponse?> GetUserByUserNameAsync(string userName, CancellationToken cancellationToken)
  {
    var user = await _userRepository.GetByUserNameAsync(userName, cancellationToken);

    if (user == null)
      return null;

    return new UserResponse
    {
      UserName = user.UserName,
      Role = user.Role.ToString()
    };
  }


}

