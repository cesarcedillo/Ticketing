using Ticketing.Auth.Application.Dtos.Responses;
using Ticketing.Auth.Application.Services.Interfaces;
using Ticketing.Auth.Domain.Interfaces;

namespace Ticketing.Auth.Application.Services;

public class AuthService : IAuthService
{
  private readonly IUserRepository _userRepository;
  private readonly IPasswordHasher _passwordHasher;
  private readonly IJwtTokenGenerator _jwtTokenGenerator;

  public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator)
  {
    _userRepository = userRepository;
    _passwordHasher = passwordHasher;
    _jwtTokenGenerator = jwtTokenGenerator;
  }

  public async Task<LoginResponse> LoginAsync(string userName, string password, CancellationToken cancellationToken)
  {
    var user = await _userRepository.GetByUserNameAsync(userName, cancellationToken);
    if (user == null || !_passwordHasher.Verify(password, user.PasswordHash))
      return LoginResponse.Failure("Invalid credentials");

    var tokenInfo = _jwtTokenGenerator.GenerateToken(user); // token y fecha

    return LoginResponse.SuccessResult(
        tokenInfo.Token,
        tokenInfo.Expiration,
        user.UserName,
        user.Role.ToString()
    );
  }


}
