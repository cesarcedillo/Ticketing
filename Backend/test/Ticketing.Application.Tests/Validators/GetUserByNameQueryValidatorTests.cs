using FluentValidation.TestHelper;
using Ticketing.Application.Queries.GetUserByName;

namespace Ticketing.Application.Tests.Validators;
public class GetUserByNameQueryValidatorTests
{
  private readonly GetUserByNameQueryValidator _validator = new();

  [Fact]
  public void Should_Have_Error_When_TicketId_Is_Empty()
  {
    var query = new GetUserByNameQuery(string.Empty);

    var result = _validator.TestValidate(query);
    result.ShouldHaveValidationErrorFor(c => c.UserName);
  }

  [Fact]
  public void Should_Not_Have_Error_When_TicketId_Is_Valid()
  {
    var query = new GetUserByNameQuery("userName");

    var result = _validator.TestValidate(query);
    result.ShouldNotHaveAnyValidationErrors();
  }
}
