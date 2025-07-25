using FluentValidation;

namespace Ticketing.BFF.Application.Commands.User.Login;
public class LoginCommandBffValidator : AbstractValidator<LoginCommandBff>
{
  public LoginCommandBffValidator()
  {
    RuleFor(x => x.UserName)
        .NotEmpty().WithMessage("Username is required.")
        .MaximumLength(100);

    RuleFor(x => x.Password)
        .NotEmpty().WithMessage("Password is required.")
        .MinimumLength(4)
        .MaximumLength(100);
  }
}
