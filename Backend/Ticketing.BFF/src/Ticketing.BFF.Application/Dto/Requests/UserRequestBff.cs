using System.Data;

namespace Ticketing.BFF.Application.Dto.Requests;
public sealed record UserRequestBff(string UserName, string Password, string Avatar, string Role);
