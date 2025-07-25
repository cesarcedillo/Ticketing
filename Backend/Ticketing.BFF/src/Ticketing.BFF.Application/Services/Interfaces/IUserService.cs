using Ticketing.BFF.Application.Dto.Responses;

namespace Ticketing.BFF.Application.Services.Interfaces;
public interface IUserService
{
  Task<LoginResponseBff> LoginAsync(string userName, string password, CancellationToken cancellationToken);
}
