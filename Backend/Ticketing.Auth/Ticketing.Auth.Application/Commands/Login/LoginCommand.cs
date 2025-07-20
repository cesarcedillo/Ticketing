using MediatR;
using Ticketing.Auth.Application.Dtos.Responses;

namespace Ticketing.Auth.Application.Commands.Login;
public sealed record LoginCommand(string UserName, string Password) : IRequest<LoginResponse>;
