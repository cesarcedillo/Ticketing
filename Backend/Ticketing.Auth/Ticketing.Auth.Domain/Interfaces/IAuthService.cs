namespace Ticketing.Auth.Domain.Interfaces;
public interface IAuthService
{
  Task<LoginResult> LoginAsync(string username, string password, CancellationToken cancellationToken = default);
}
