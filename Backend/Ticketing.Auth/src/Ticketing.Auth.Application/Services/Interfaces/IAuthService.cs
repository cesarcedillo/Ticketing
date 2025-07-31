using Ticketing.Auth.Application.Dtos.Responses;
using Ticketing.Auth.Domain.Enums;

namespace Ticketing.Auth.Application.Services.Interfaces;
public interface IAuthService
{
  Task<SignInResponse> SignInAsync(string userName, string password, CancellationToken cancellationToken);

  Task<SignInResponse> SignUpAsync(string userName, string password, Role role, CancellationToken cancellationToken);
}
