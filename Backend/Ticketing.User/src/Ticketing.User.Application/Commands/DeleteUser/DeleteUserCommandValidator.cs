using FluentValidation;

namespace Ticketing.User.Application.Commands.DeleteUser;
public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{  public DeleteUserCommandValidator()
  {
    RuleFor(x => x.UserName)
        .NotEmpty().WithMessage("User name is required.");
  }
}
