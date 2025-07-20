using MediatR;
using Ticketing.Auth.Application.Dtos.Responses;
using Ticketing.Auth.Application.Services.Interfaces;

namespace Ticketing.Auth.Application.Commands.Login;
public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
  private readonly IAuthService _authService;

  public LoginCommandHandler(IAuthService authService)
  {
    _authService = authService;
  }

  public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
  {
    return await _authService.LoginAsync(request.UserName, request.Password, cancellationToken);
  }
}
