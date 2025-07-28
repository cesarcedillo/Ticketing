using FluentValidation;
using Ticketing.User.Domain.Enums;

namespace Ticketing.User.Application.Commands.CreateUser;
public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
  public CreateUserCommandValidator()
  {
    RuleFor(x => x.UserName)
        .NotEmpty().WithMessage("UserName is required.")
        .MaximumLength(100).WithMessage("UserName cannot be longer than 100 characters."); 

    RuleFor(x => x.Avatar)
        .NotEmpty().WithMessage("Avatar is required.")
        .MaximumLength(4000).WithMessage("Avatar cannot be longer than 4000 characters.");

    RuleFor(x => x.Role)
        .NotEmpty().WithMessage("Role is required.")
        .IsEnumName(typeof(Role), caseSensitive: false)
        .WithMessage("Role must be a valid value.");
  }

}
