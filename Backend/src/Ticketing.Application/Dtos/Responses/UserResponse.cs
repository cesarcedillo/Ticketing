using Ticketing.Domain.Enums;

namespace Ticketing.Application.Dtos.Responses;
public sealed record UserResponse
{
  public string UserName { get; set; } = string.Empty;
  public byte[] Avatar { get; set; } = [];
  public string Type { get;  set; } = UserType.Customer.ToString();
}
