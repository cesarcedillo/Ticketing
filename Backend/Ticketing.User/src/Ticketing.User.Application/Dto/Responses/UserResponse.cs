namespace Ticketing.User.Application.Dto.Responses;

public sealed record UserResponse(Guid Id, string UserName, string Avatar, string Type);

