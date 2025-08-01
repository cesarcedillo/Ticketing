using Ticketing.BFF.Application.Dto.Responses;

namespace Ticketing.BFF.Application.Services.Interfaces;
public interface IUserService
{
  Task<LoginResponseBff> LoginAsync(string userName, string password, CancellationToken cancellationToken);
  Task<UserResponseBff> GetUserByUserNameAsync(string userName, CancellationToken cancellationToken);
  Task<UserResponseBff> CreateUserAsync(string userName, string password, string avatar, string role, CancellationToken cancellationToken);
}
