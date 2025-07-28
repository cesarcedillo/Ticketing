using FluentValidation;

namespace Ticketing.BFF.Application.Commands.User.CreateUser;
public class CreateUserCommandBffValidator : AbstractValidator<CreateUserCommandBff>
{
  public CreateUserCommandBffValidator()
  {
    RuleFor(x => x.UserName)
        .NotEmpty().WithMessage("UserName is required.");

    RuleFor(x => x.Avatar)
        .NotEmpty().WithMessage("Avatar is required.");

    RuleFor(x => x.Role)
        .NotEmpty().WithMessage("Role is required.");
  }

}
