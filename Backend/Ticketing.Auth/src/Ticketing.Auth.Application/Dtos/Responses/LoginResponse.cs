using Ticketing.Core.Domain.SeedWork.Interfaces;

namespace Ticketing.Auth.Application.Dtos.Responses;

public class LoginResponse
{
  public bool Success { get; set; }
  public string? Message { get; set; }

  public string? AccessToken { get; set; }
  public DateTime? Expiration { get; set; }
  public string? UserName { get; set; }
  public string? Role { get; set; }

  public static LoginResponse Failure(string message)
      => new LoginResponse { Success = false, Message = message };

  public static LoginResponse SuccessResult(string accessToken, DateTime expiration, string userName, string role)
      => new LoginResponse
      {
        Success = true,
        AccessToken = accessToken,
        Expiration = expiration,
        UserName = userName,
        Role = role
      };
}
