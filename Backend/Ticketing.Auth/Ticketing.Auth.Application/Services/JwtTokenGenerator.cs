using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ticketing.Auth.Application.Dtos;
using Ticketing.Auth.Application.Services.Interfaces;
using Ticketing.Auth.Domain.Aggregates;

namespace Ticketing.Auth.Application.Services;

public class JwtTokenGenerator : IJwtTokenGenerator
{
  private readonly IConfiguration _configuration;
  public JwtTokenGenerator(IConfiguration configuration)
  {
    _configuration = configuration;
  }

  public JwtTokenDto GenerateToken(User user)
  {
    var jwtSecret = _configuration["Jwt:Secret"] ?? "SOME_SECRET_KEY";
    var expiryHours = double.TryParse(_configuration["Jwt:ExpiryHours"], out var h) ? h : 2;
    var expires = DateTime.UtcNow.AddHours(expiryHours);

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

    string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

    return new JwtTokenDto(tokenString, expires);

  }
}
