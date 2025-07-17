using Ticketing.Domain.Enums;

namespace Ticketing.Application.Dtos.Responses;
public sealed record UserResponse
{
  public string UserName { get; set; } = string.Empty;
  public byte[] Avatar { get; set; } = [];
  public UserType UserType { get; set; } = UserType.Customer;
}
