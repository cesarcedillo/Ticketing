using FluentValidation.TestHelper;
using Ticketing.Ticket.Application.Queries.ListTickets;

namespace Ticketing.Ticket.Application.Tests.Validators;

public class ListTicketsQueryValidatorTests
{
  private readonly ListTicketsQueryValidator _validator = new();

  [Fact]
  public void Should_Not_Have_Error_When_All_Fields_Are_Null()
  {
    var query = new ListTicketsQuery(null, null);

    var result = _validator.TestValidate(query);
    result.ShouldNotHaveAnyValidationErrors();
  }

  [Fact]
  public void Should_Have_Error_When_Status_Is_Invalid()
  {
    var query = new ListTicketsQuery("InvalidStatus", null);

    var result = _validator.TestValidate(query);
    result.ShouldHaveValidationErrorFor(q => q.Status);
  }

  [Fact]
  public void Should_Not_Have_Error_When_Status_And_UserId_Are_Valid()
  {
    var query = new ListTicketsQuery("Open", Guid.NewGuid());

    var result = _validator.TestValidate(query);
    result.ShouldNotHaveAnyValidationErrors();
  }
}
