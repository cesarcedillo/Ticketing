using FluentValidation;

namespace Ticketing.Auth.Application.Commands.SignIn;
public class SignInCommandValidator : AbstractValidator<SignInCommand>
{
  public SignInCommandValidator()
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
