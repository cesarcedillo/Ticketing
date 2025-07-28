using FluentValidation;

namespace Ticketing.User.Application.Queries.GetUsersByIds;
public class GetUsersByIdsQueryValidator : AbstractValidator<GetUsersByIdsQuery>
{
  public GetUsersByIdsQueryValidator()
  {
    RuleFor(q => q.UserIds)
        .NotNull().WithMessage("UserIds list must not be null")
        .NotEmpty().WithMessage("UserIds list must not be empty");
    RuleForEach(q => q.UserIds)
        .NotEqual(Guid.Empty).WithMessage("UserIds cannot contain Guid.Empty");
  }
}
