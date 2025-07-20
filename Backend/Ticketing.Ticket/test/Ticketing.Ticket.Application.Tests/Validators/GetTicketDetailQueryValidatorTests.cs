using FluentValidation.TestHelper;
using Ticketing.Ticket.Application.Queries.GetTicketDetail;

namespace Ticketing.Ticket.Application.Tests.Validators;
public class GetTicketDetailQueryValidatorTests
{
  private readonly GetTicketDetailQueryValidator _validator = new();

  [Fact]
  public void Should_Have_Error_When_TicketId_Is_Empty()
  {
    var query = new GetTicketDetailQuery(Guid.Empty);

    var result = _validator.TestValidate(query);
    result.ShouldHaveValidationErrorFor(c => c.TicketId);
  }

  [Fact]
  public void Should_Not_Have_Error_When_TicketId_Is_Valid()
  {
    var query = new GetTicketDetailQuery(Guid.NewGuid());

    var result = _validator.TestValidate(query);
    result.ShouldNotHaveAnyValidationErrors();
  }
}
