using MediatR;
using Ticketing.Auth.Application.Dtos.Responses;

namespace Ticketing.Auth.Application.Commands.SignIn;
public sealed record SignInCommand(string UserName, string Password) : IRequest<SignInResponse>;
