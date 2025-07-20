using Ticketing.Auth.Application.Dtos.Responses;

namespace Ticketing.Auth.Application.Services.Interfaces;
public interface IUserService
{
  Task<UserResponse> GetUserByUserNameAsync(string userName, CancellationToken cancellationToken);
}
