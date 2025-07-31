using FluentValidation.TestHelper;
using Ticketing.User.Application.Commands.DeleteUser;
using Xunit;

namespace Ticketing.User.Application.Tests.Validators;

public class DeleteUserCommandValidatorTests
{
  private readonly DeleteUserCommandValidator _validator = new();

  [Fact]
  public void Should_Have_Error_When_UserName_Is_Empty()
  {
    var command = new DeleteUserCommand("");
    var result = _validator.TestValidate(command);
    result.ShouldHaveValidationErrorFor(x => x.UserName)
          .WithErrorMessage("User name is required.");
  }

  [Fact]
  public void Should_Not_Have_Error_When_UserName_Is_Provided()
  {
    var command = new DeleteUserCommand("validUser");
    var result = _validator.TestValidate(command);
    result.ShouldNotHaveAnyValidationErrors();
  }
}
