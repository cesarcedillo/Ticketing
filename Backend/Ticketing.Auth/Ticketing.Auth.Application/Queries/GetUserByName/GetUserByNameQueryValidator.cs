using FluentValidation;

namespace Ticketing.Auth.Application.Queries.GetUserByName;
public class GetUserByNameQueryValidator : AbstractValidator<GetUserByNameQuery>
{
  public GetUserByNameQueryValidator()
  {
    RuleFor(x => x.UserName)
        .NotEmpty().WithMessage("Username is required.")
        .MaximumLength(100);
  }
}
