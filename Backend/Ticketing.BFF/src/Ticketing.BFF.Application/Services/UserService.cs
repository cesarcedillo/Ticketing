using Auth.Cliente.NswagAutoGen.HttpClientFactoryImplementation;
using AutoMapper;
using Ticketing.BFF.Application.Dto.Responses;
using Ticketing.BFF.Application.Services.Interfaces;
using User.Cliente.NswagAutoGen.HttpClientFactoryImplementation;

namespace Ticketing.BFF.Application.Services;

public class UserService : IUserService
{
  private readonly IAuthClient _authClient;
  private readonly IUserClient _userClient;
  private readonly IMapper _mapper;

  public UserService(IAuthClient authClient, IUserClient userClient, IMapper mapper)
  {
    _authClient = authClient;
    _userClient = userClient;
    _mapper = mapper;
  }

  public async Task<LoginResponseBff> LoginAsync(string userName, string password, CancellationToken cancellationToken)
  {
    var loginRequest = new LoginRequest
    {
      Username = userName,
      Password = password
    };
    var loginResponse = _mapper.Map<LoginResponseBff>(await _authClient.LoginAsync(loginRequest, cancellationToken));

    if (loginResponse.Success)
    {

      _authClient.BearerToken = "";
      loginResponse.User = _mapper.Map<UserResponseBff>(await _userClient.GetUserByUserNameAsync(userName, cancellationToken));
    }

    return loginResponse;
  }

}

