using Ticketing.Ticket.Domain.Enums;

namespace Ticketing.Ticket.Application.Dtos.Responses;
public sealed record UserResponse
{
  public Guid Id { get; set; }
  public string UserName { get; set; } = string.Empty;
  public string Avatar { get; set; } = string.Empty;
  public string Type { get;  set; } = UserType.Customer.ToString();
}
