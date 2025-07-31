using FluentValidation.TestHelper;
using Ticketing.User.Application.Commands.CreateUser;

namespace Ticketing.User.Application.Tests.Validators;

public class CreateUserCommandValidatorTests
{
  private readonly CreateUserCommandValidator _validator = new();

  [Fact]
  public void Should_Have_Error_When_UserName_Is_Empty()
  {
    var command = new CreateUserCommand("", "avatar", "Customer");
    var result = _validator.TestValidate(command);
    result.ShouldHaveValidationErrorFor(x => x.UserName);
  }

  [Fact]
  public void Should_Have_Error_When_UserName_Exceeds_MaxLength()
  {
    var longUserName = new string('a', 101);
    var command = new CreateUserCommand(longUserName, "avatar", "Customer");
    var result = _validator.TestValidate(command);
    result.ShouldHaveValidationErrorFor(x => x.UserName);
  }

  [Fact]
  public void Should_Have_Error_When_Avatar_Is_Empty()
  {
    var command = new CreateUserCommand("user", "", "Customer");
    var result = _validator.TestValidate(command);
    result.ShouldHaveValidationErrorFor(x => x.Avatar);
  }

  [Fact]
  public void Should_Have_Error_When_Avatar_Exceeds_MaxLength()
  {
    var longAvatar = new string('x', 4001);
    var command = new CreateUserCommand("user", longAvatar, "Customer");
    var result = _validator.TestValidate(command);
    result.ShouldHaveValidationErrorFor(x => x.Avatar);
  }

  [Fact]
  public void Should_Have_Error_When_Role_Is_Empty()
  {
    var command = new CreateUserCommand("user", "avatar", "");
    var result = _validator.TestValidate(command);
    result.ShouldHaveValidationErrorFor(x => x.Role);
  }

  [Fact]
  public void Should_Have_Error_When_Role_Is_Invalid()
  {
    var command = new CreateUserCommand("user", "avatar", "NotARole");
    var result = _validator.TestValidate(command);
    result.ShouldHaveValidationErrorFor(x => x.Role)
          .WithErrorMessage("Role must be a valid value.");
  }

  [Fact]
  public void Should_Not_Have_Errors_When_All_Fields_Are_Valid()
  {
    var command = new CreateUserCommand("user", "avatar", "Agent");
    var result = _validator.TestValidate(command);
    result.ShouldNotHaveAnyValidationErrors();
  }

  [Fact]
  public void Should_Not_Have_Error_When_Role_Is_Valid_CaseInsensitive()
  {
    var command = new CreateUserCommand("user", "avatar", "admin");
    var result = _validator.TestValidate(command);
    result.ShouldNotHaveValidationErrorFor(x => x.Role);
  }
}
