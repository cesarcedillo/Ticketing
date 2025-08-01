using Auth.Cliente.NswagAutoGen.HttpClientFactoryImplementation;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Ticketing.BFF.Application.Dto.Responses;
using Ticketing.BFF.Application.Services.Interfaces;
using User.Cliente.NswagAutoGen.HttpClientFactoryImplementation;

namespace Ticketing.BFF.Application.Services;

public class UserService : IUserService
{
  private readonly IAuthClient _authClient;
  private readonly IUserClient _userClient;
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly IMapper _mapper;

  public UserService(IAuthClient authClient, IUserClient userClient, IHttpContextAccessor httpContextAccessor, IMapper mapper)
  {
    _authClient = authClient;
    _userClient = userClient;
    _httpContextAccessor = httpContextAccessor;
    _mapper = mapper;
  }

  public async Task<LoginResponseBff> LoginAsync(string userName, string password, CancellationToken cancellationToken)
  {
    var singInRequest = new SingInRequest
    {
      Username = userName,
      Password = password
    };
    var loginResponse = _mapper.Map<LoginResponseBff>(await _authClient.SignInAsync(singInRequest, cancellationToken));

    if (loginResponse.Success && !string.IsNullOrEmpty(loginResponse.AccessToken))
    {
      _httpContextAccessor.HttpContext!.Items["ManualToken"] = loginResponse.AccessToken;
      loginResponse.User = _mapper.Map<UserResponseBff>(await _userClient.GetUserByUserNameAsync(userName, cancellationToken));
      _httpContextAccessor.HttpContext!.Items.Remove("ManualToken");
    }

    return loginResponse;
  }

  public async Task<UserResponseBff> GetUserByUserNameAsync(string userName, CancellationToken cancellationToken)
  {
    var user = await _userClient.GetUserByUserNameAsync(userName, cancellationToken);
    var userResponse = _mapper.Map<UserResponseBff>(user);
    return userResponse;
  }

  public async Task<UserResponseBff> CreateUserAsync(string userName, string password, string avatar, string role, CancellationToken cancellationToken)
  {
    var userRequest = new UserRequest
    {
      UserName = userName,
      Avatar = password,
      Role = role
    };

    var createdUser = await _userClient.CreateUserAsync(userRequest, cancellationToken);

    try
    {
      var signUpRequest = new SingUpRequest
      {
        Username = userName,
        Password = password
      };
      await _authClient.SignUpAsync(signUpRequest, cancellationToken);

      return new UserResponseBff
      {
        Id = createdUser.Id!.Value.ToString(),
        UserName = createdUser.UserName!,
        Avatar = createdUser.Avatar!,
        Type = createdUser.Type!
      };
    }
    catch (Exception ex)
    {
      await _userClient.DeleteUserAsync(userName, cancellationToken);
      throw new InvalidOperationException("Error creating credentials. User rolled back.", ex);
    }
  }
}

