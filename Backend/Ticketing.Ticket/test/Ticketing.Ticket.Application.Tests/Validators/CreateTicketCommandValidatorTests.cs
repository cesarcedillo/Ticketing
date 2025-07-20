using FluentValidation.TestHelper;
using Ticketing.Ticket.Application.Commands.CreateTicket;

namespace Ticketing.Ticket.Application.Tests.Validators
{
  public class CreateTicketCommandValidatorTests
  {
    private readonly CreateTicketCommandValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Subject_Is_Empty()
    {
      var command = new CreateTicketCommand("", "Description", Guid.NewGuid());

      var result = _validator.TestValidate(command);
      result.ShouldHaveValidationErrorFor(c => c.Subject);
    }

    [Fact]
    public void Should_Have_Error_When_Description_Is_Empty()
    {
      var command = new CreateTicketCommand("Subject", "", Guid.NewGuid());

      var result = _validator.TestValidate(command);
      result.ShouldHaveValidationErrorFor(c => c.Description);
    }

    [Fact]
    public void Should_Have_Error_When_UserId_Is_Empty()
    {
      var command = new CreateTicketCommand("Subject", "Description", Guid.Empty);

      var result = _validator.TestValidate(command);
      result.ShouldHaveValidationErrorFor(c => c.UserId);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
      var command = new CreateTicketCommand("Subject", "Description", Guid.NewGuid());

      var result = _validator.TestValidate(command);
      result.ShouldNotHaveAnyValidationErrors();
    }
  }
}
