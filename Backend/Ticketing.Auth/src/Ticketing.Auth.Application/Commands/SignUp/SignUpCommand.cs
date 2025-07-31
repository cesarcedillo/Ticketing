using MediatR;
using Ticketing.Auth.Application.Dtos.Responses;

namespace Ticketing.Auth.Application.Commands.SignUp;
public sealed record SignUpCommand(string UserName, string Password, string Role) : IRequest<SignInResponse>;
