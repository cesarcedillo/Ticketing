using FluentValidation.TestHelper;
using Ticketing.Ticket.Application.Commands.MarkTicketAsResolved;

namespace Ticketing.Ticket.Application.Tests.Validators
{
  public class MarkTicketAsResolvedCommandValidatorTests
  {
    private readonly MarkTicketAsResolvedCommandValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_TicketId_Is_Empty()
    {
      var command = new MarkTicketAsResolvedCommand(Guid.Empty);

      var result = _validator.TestValidate(command);
      result.ShouldHaveValidationErrorFor(c => c.TicketId);
    }

    [Fact]
    public void Should_Not_Have_Error_When_TicketId_Is_Valid()
    {
      var command = new MarkTicketAsResolvedCommand(Guid.NewGuid());

      var result = _validator.TestValidate(command);
      result.ShouldNotHaveAnyValidationErrors();
    }
  }
}
