using Auth.Cliente.NswagAutoGen.HttpClientFactoryImplementation;
using AutoMapper;
using MediatR;
using Ticketing.BFF.Application.Dto.Responses;
using Ticketing.BFF.Application.Services.Interfaces;

namespace Ticketing.BFF.Application.Commands.User.Login;
public class LoginCommandBffHandler : IRequestHandler<LoginCommandBff, LoginResponseBff>
{
  private readonly IUserService _userService;

  public LoginCommandBffHandler(IUserService userService)
  {
    _userService = userService;
  }

  public async Task<LoginResponseBff> Handle(LoginCommandBff request, CancellationToken cancellationToken)
  {
    return await _userService.LoginAsync(request.UserName, request.Password, cancellationToken);
  }
}
