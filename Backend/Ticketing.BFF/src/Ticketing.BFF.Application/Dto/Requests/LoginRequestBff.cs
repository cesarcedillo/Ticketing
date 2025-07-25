namespace Ticketing.BFF.Application.Dto.Requests;
public class LoginRequestBff
{
  public string Username { get; set; } = default!;
  public string Password { get; set; } = default!;
}
