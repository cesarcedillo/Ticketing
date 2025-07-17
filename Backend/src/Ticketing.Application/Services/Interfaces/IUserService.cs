using Ticketing.Application.Dtos.Responses;

namespace Ticketing.Application.Services.Interfaces
{
  public interface IUserService
  {
    Task<UserResponse> GetUserByUserNameAsync(string userName, CancellationToken cancellationToken);
  }
}
