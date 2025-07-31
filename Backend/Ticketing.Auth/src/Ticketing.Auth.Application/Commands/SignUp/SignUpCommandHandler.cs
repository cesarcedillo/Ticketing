using MediatR;
using Ticketing.Auth.Application.Commands.SignIn;
using Ticketing.Auth.Application.Dtos.Responses;
using Ticketing.Auth.Application.Services.Interfaces;
using Ticketing.Auth.Domain.Enums;

namespace Ticketing.Auth.Application.Commands.SignUp;
public class SignUpCommandHandler : IRequestHandler<SignUpCommand, SignInResponse>
{
  private readonly IAuthService _authService;

  public SignUpCommandHandler(IAuthService authService)
  {
    _authService = authService;
  }

  public async Task<SignInResponse> Handle(SignUpCommand request, CancellationToken cancellationToken)
  {
    var roleEnum = (Role)Enum.Parse(typeof(Role), request.Role, ignoreCase: true);

    return await _authService.SignUpAsync(request.UserName, request.Password, roleEnum, cancellationToken);
  }
}
