using Ticketing.User.Domain.Enums;

namespace Ticketing.User.Application.Dto.Responses;

public sealed record UserResponse
{
  public Guid Id { get; set; }
  public string UserName { get; set; } = string.Empty;
  public string Avatar { get; set; } = string.Empty;
  public string Type { get; set; } = Role.Customer.ToString();
}
