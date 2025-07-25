
namespace Ticketing.BFF.Application.Dto.Responses;
public class LoginResponseBff
{
  public bool Success { get; set; }
  public string? Message { get; set; }

  public string? AccessToken { get; set; }
  public DateTime? Expiration { get; set; }
  public UserResponseBff? User { get; set; }
}