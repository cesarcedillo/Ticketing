namespace Ticketing.BFF.Application.Dto.Responses;
public sealed record UserResponseBff
{
  public string UserName { get; set; } = string.Empty;
  public string Avatar { get; set; } = string.Empty;
  public string Type { get; set; } = string.Empty;
}