using Ticketing.Core.Domain.SeedWork.Interfaces;

namespace Ticketing.Auth.Application.Dtos.Requests;
public class LoginRequest
{
  public string Username { get; set; } = default!;
  public string Password { get; set; } = default!;
}
