using Ticketing.Ticket.Application.Dtos.Responses;

namespace Ticketing.Ticket.Application.Services.Interfaces
{
  public interface IUserService
  {
    Task<UserResponse> GetUserByUserNameAsync(string userName, CancellationToken cancellationToken);
  }
}
