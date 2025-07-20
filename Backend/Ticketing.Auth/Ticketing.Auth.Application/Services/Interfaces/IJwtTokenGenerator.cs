
using Ticketing.Auth.Application.Dtos;
using Ticketing.Auth.Domain.Aggregates;

namespace Ticketing.Auth.Application.Services.Interfaces;
public interface IJwtTokenGenerator
{
  JwtTokenDto GenerateToken(User user);
}
