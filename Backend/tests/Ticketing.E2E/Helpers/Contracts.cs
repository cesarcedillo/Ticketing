namespace Ticketing.E2E.Helpers;

public sealed record LoginResponseBff(
  bool Success,
  string? Message,
  string? AccessToken,
  DateTimeOffset? Expiration,
  UserResponseBff? User);

public sealed record UserResponseBff(
  string? Id,
  string? UserName,
  string? Avatar,
  string? Type);

public sealed record TicketDetailResponseBff(
  Guid Id,
  string? Subject,
  string? Description,
  string? Status,
  UserResponseBff? User,
  List<TicketReplyResponseBff>? Replies);

public sealed record TicketReplyResponseBff(
  Guid Id,
  string? Text,
  DateTimeOffset CreatedAt,
  UserResponseBff? User);

public sealed record ProblemDetails(
  string? Type,
  string? Title,
  int? Status,
  string? Detail,
  string? Instance);
