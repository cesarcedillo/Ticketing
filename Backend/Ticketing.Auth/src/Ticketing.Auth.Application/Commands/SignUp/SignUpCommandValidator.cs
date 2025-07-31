using FluentValidation;
using Ticketing.Auth.Domain.Enums;

namespace Ticketing.Auth.Application.Commands.SignUp;
public class SignUpCommandValidator : AbstractValidator<SignUpCommand>
{
  public SignUpCommandValidator()
  {
    RuleFor(x => x.UserName)
        .NotEmpty().WithMessage("Username is required.")
        .MaximumLength(100);

    RuleFor(x => x.Password)
        .NotEmpty().WithMessage("Password is required.")
        .MinimumLength(4)
        .MaximumLength(100);

    RuleFor(x => x.Role)
        .NotEmpty().WithMessage("Role is required.")
        .Must(BeAValidRole)
        .WithMessage("Role must be one of the defined values: Admin, Agent, Customer.");
  }

  private bool BeAValidRole(string role)
  {
    return Enum.GetNames(typeof(Role)).Any(name => name.Equals(role, StringComparison.OrdinalIgnoreCase));
  }


}