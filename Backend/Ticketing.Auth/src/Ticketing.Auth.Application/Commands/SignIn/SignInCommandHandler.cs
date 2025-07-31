using MediatR;
using Ticketing.Auth.Application.Dtos.Responses;
using Ticketing.Auth.Application.Services.Interfaces;

namespace Ticketing.Auth.Application.Commands.SignIn;
public class SignInCommandHandler : IRequestHandler<SignInCommand, SignInResponse>
{
  private readonly IAuthService _authService;

  public SignInCommandHandler(IAuthService authService)
  {
    _authService = authService;
  }

  public async Task<SignInResponse> Handle(SignInCommand request, CancellationToken cancellationToken)
  {
    return await _authService.SignInAsync(request.UserName, request.Password, cancellationToken);
  }
}
