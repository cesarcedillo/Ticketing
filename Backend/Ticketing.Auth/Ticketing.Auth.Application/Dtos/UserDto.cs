using Ticketing.Core.Domain.SeedWork.Interfaces;

namespace Ticketing.Auth.Application.Dtos;
public class UserDto : IDto
{
  public string Username { get; set; } = default!;
  public string Role { get; set; } = default!;
}
