using MediatR;
using Ticketing.BFF.Application.Dto.Responses;

namespace Ticketing.BFF.Application.Commands.User.Login;
public sealed record LoginCommandBff(string UserName, string Password) : IRequest<LoginResponseBff>;