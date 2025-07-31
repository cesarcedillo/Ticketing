using Ticketing.User.Application.Dto.Responses;

namespace Ticketing.User.Application.Services.Interfaces;
public interface IUserService
{
  Task<UserResponse> GetUserByUserNameAsync(string userName, CancellationToken cancellationToken);
  Task<IEnumerable<UserResponse>> GetUsersByIdsAsync(IEnumerable<Guid> userIds, CancellationToken cancellationToken);
  Task<UserResponse> CreateUserAsync(string userName, string avatar, string role, CancellationToken cancellationToken);
  Task DeleteUserAsync(string userName, CancellationToken cancellationToken);

}
