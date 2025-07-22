using Ticketing.User.Application.Dto.Responses;

namespace Ticketing.User.Application.Services.Interfaces;
public interface IUserService
{
  Task<UserResponse> GetUserByUserNameAsync(string userName, CancellationToken cancellationToken);
}
