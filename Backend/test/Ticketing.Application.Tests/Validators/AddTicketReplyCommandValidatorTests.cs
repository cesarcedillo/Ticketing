using FluentValidation.TestHelper;
using Ticketing.Application.Commands.AddTicketReply;

namespace Ticketing.Application.Tests.Validators;
  public class AddTicketReplyCommandValidatorTests
{
  private readonly AddTicketReplyCommandValidator _validator = new();

  [Fact]
  public void Should_Have_Error_When_Text_Is_Empty()
  {
    var command = new AddTicketReplyCommand(Guid.NewGuid(), "", Guid.NewGuid());

    var result = _validator.TestValidate(command);
    result.ShouldHaveValidationErrorFor(c => c.Text);
  }

  [Fact]
  public void Should_Have_Error_When_TicketId_Is_Empty()
  {
    var command = new AddTicketReplyCommand(Guid.Empty, "Some reply", Guid.NewGuid());

    var result = _validator.TestValidate(command);
    result.ShouldHaveValidationErrorFor(c => c.TicketId);
  }

  [Fact]
  public void Should_Have_Error_When_UserId_Is_Empty()
  {
    var command = new AddTicketReplyCommand(Guid.NewGuid(), "Some reply", Guid.Empty);

    var result = _validator.TestValidate(command);
    result.ShouldHaveValidationErrorFor(c => c.UserId);
  }

  [Fact]
  public void Should_Not_Have_Error_When_Command_Is_Valid()
  {
    var command = new AddTicketReplyCommand(Guid.NewGuid(), "A valid reply", Guid.NewGuid());

    var result = _validator.TestValidate(command);
    result.ShouldNotHaveAnyValidationErrors();
  }
}
