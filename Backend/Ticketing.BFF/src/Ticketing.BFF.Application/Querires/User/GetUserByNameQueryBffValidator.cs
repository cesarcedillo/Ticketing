using FluentValidation;

namespace Ticketing.BFF.Application.Querires.User;
public class GetUserByNameQueryBffValidator : AbstractValidator<GetUserByNameQueryBff>
{
  public GetUserByNameQueryBffValidator()
  {
    RuleFor(x => x.UserName)
        .NotEmpty().WithMessage("User name is required.");
  }
}