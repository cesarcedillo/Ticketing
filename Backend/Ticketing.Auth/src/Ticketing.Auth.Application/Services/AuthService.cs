using Ticketing.Auth.Application.Dtos.Responses;
using Ticketing.Auth.Application.Services.Interfaces;
using Ticketing.Auth.Domain.Aggregates;
using Ticketing.Auth.Domain.Enums;
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

  public async Task<SignInResponse> SignInAsync(string userName, string password, CancellationToken cancellationToken)
  {
    var user = await _userRepository.GetByUserNameAsync(userName, cancellationToken);
    if (user == null || !_passwordHasher.Verify(password, user.PasswordHash))
      return SignInResponse.Failure("Invalid credentials");

    var tokenInfo = _jwtTokenGenerator.GenerateToken(user); // token y fecha

    return SignInResponse.SuccessResult(
        tokenInfo.Token,
        tokenInfo.Expiration,
        user.UserName,
        user.Role.ToString()
    );
  }

  public async Task<SignInResponse> SignUpAsync(string userName, string password, Role role, CancellationToken cancellationToken)
  {
    var existingUser = await _userRepository.GetByUserNameAsync(userName, cancellationToken);
    if (existingUser is not null)
      throw new InvalidOperationException($"User with username '{userName}' already exists.");

    var hashedPassword = _passwordHasher.Hash(password);
    var user = new User(userName, hashedPassword, role);
    await _userRepository.AddAsync(user, cancellationToken);
    var token = _jwtTokenGenerator.GenerateToken(user);

    return SignInResponse.SuccessResult(
        token.Token,
        token.Expiration,
        user.UserName,
        user.Role.ToString()
    );
  }
}
