namespace Ticketing.Auth.Application.Dtos.Requests;
public class SingUpRequest
{
  public string Username { get; set; } = default!;
  public string Role { get; set; } = default!;
  public string Password { get; set; } = default!;
}
