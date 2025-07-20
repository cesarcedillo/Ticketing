using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Ticketing.Auth.Application.Dtos.Responses;
using Ticketing.Auth.Application.Services.Interfaces;
using Ticketing.Auth.Domain.Aggregates;
using Ticketing.Auth.Domain.Interfaces;

namespace Ticketing.Auth.Application.Services;

public class AuthService : IAuthService
{
  private readonly IUserRepository _userRepository;
  private readonly IConfiguration _configuration;

  public AuthService(IUserRepository userRepository, IConfiguration configuration)
  {
    _userRepository = userRepository;
    _configuration = configuration;
  }

  public async Task<LoginResponse> LoginAsync(string userName, string password, CancellationToken cancellationToken)
  {
    var user = await _userRepository.GetByUserNameAsync(userName, cancellationToken);

    if (user == null || !VerifyPassword(password, user.PasswordHash))
      return LoginResponse.Failure("Invalid credentials");

    var expirationDate = DateTime.UtcNow.AddHours(2);

    var token = GenerateJwtToken(user);

    return LoginResponse.SuccessResult(
        token,
        expirationDate,
        user.UserName,
        user.Role.ToString()
    );
  }

  private bool VerifyPassword(string password, string storedHash)
  {
    using var sha = SHA256.Create();
    var passwordBytes = Encoding.UTF8.GetBytes(password);
    var computedHash = sha.ComputeHash(passwordBytes);
    var storedHashBytes = Convert.FromBase64String(storedHash);

    return CryptographicOperations.FixedTimeEquals(computedHash, storedHashBytes);

  }

  private string GenerateJwtToken(User user)
  {
    var jwtSecret = _configuration["Jwt:Secret"] ?? "SOME_SECRET_KEY";
    var expires = DateTime.UtcNow.AddHours(
        double.TryParse(_configuration["Jwt:ExpiryHours"], out var h) ? h : 2
    );

    var claims = new[]
    {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: _configuration["Jwt:Issuer"],
        audience: _configuration["Jwt:Audience"],
        claims: claims,
        expires: expires,
        signingCredentials: creds
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
  }


}
