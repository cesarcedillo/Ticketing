using Ticketing.Auth.Application.Dtos.Responses;

namespace Ticketing.Auth.Application.Services.Interfaces;
public interface IAuthService
{
  Task<LoginResponse> LoginAsync(string userName, string password, CancellationToken cancellationToken);
}
