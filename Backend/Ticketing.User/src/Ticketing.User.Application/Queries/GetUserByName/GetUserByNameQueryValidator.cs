using FluentValidation;

namespace Ticketing.User.Application.Queries.GetUserByName;
public class GetUserByNameQueryValidator : AbstractValidator<GetUserByNameQuery>
{
  public GetUserByNameQueryValidator()
  {
    RuleFor(x => x.UserName)
        .NotEmpty().WithMessage("User name is required.");
  }
}