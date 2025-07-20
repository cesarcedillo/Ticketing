using Ticketing.Auth.Domain.Aggregates;
using Ticketing.Core.Domain.SeedWork.Interfaces;

namespace Ticketing.Auth.Domain.ValueObjects;
public class LoginResult : IDto
{
  public bool Success { get; }
  public User? User { get; }
  public string? ErrorMessage { get; }

  private LoginResult(bool success, User? user, string? errorMessage)
  {
    Success = success;
    User = user;
    ErrorMessage = errorMessage;
  }

  public static LoginResult SuccessResult(User user) => new(true, user, null);
  public static LoginResult Fail(string error) => new(false, null, error);
}
