using FluentValidation;

namespace Ticketing.Application.Queries.GetUserByName;
public class GetUserByNameQueryValidator : AbstractValidator<GetUserByNameQuery>
{
  public GetUserByNameQueryValidator()
  {
    RuleFor(x => x.UserName)
        .NotEmpty().WithMessage("User name is required.");
  }
}
