using Ticketing.Core.Domain.SeedWork.Interfaces;

namespace Ticketing.Auth.Application.Dtos.Responses;

public class SignInResponse
{
  public bool Success { get; set; }
  public string? Message { get; set; }

  public string? AccessToken { get; set; }
  public DateTime? Expiration { get; set; }
  public string? UserName { get; set; }
  public string? Role { get; set; }

  public static SignInResponse Failure(string message)
      => new SignInResponse { Success = false, Message = message };

  public static SignInResponse SuccessResult(string accessToken, DateTime expiration, string userName, string role)
      => new SignInResponse
      {
        Success = true,
        AccessToken = accessToken,
        Expiration = expiration,
        UserName = userName,
        Role = role
      };
}
