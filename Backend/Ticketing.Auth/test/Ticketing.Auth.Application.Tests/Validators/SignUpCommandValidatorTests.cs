using FluentValidation.TestHelper;
using Ticketing.Auth.Application.Commands.SignUp;
using Ticketing.Auth.Domain.Enums;

namespace Ticketing.Auth.Application.Tests.Validators;
public class SignUpCommandValidatorTests
{
  private readonly SignUpCommandValidator _validator = new();

  [Fact]
  public void Should_Have_Error_When_Username_Is_Empty()
  {
    var model = new SignUpCommand("", "password123", Role.Customer.ToString());
    var result = _validator.TestValidate(model);
    result.ShouldHaveValidationErrorFor(x => x.UserName);
  }

  [Fact]
  public void Should_Have_Error_When_Username_Exceeds_MaxLength()
  {
    var longUsername = new string('a', 101);
    var model = new SignUpCommand(longUsername, "password123", Role.Customer.ToString());
    var result = _validator.TestValidate(model);
    result.ShouldHaveValidationErrorFor(x => x.UserName);
  }

  [Fact]
  public void Should_Have_Error_When_Password_Is_Empty()
  {
    var model = new SignUpCommand("user", "", Role.Customer.ToString());
    var result = _validator.TestValidate(model);
    result.ShouldHaveValidationErrorFor(x => x.Password);
  }

  [Fact]
  public void Should_Have_Error_When_Password_Too_Short()
  {
    var model = new SignUpCommand("user", "123", Role.Customer.ToString());
    var result = _validator.TestValidate(model);
    result.ShouldHaveValidationErrorFor(x => x.Password);
  }

  [Fact]
  public void Should_Have_Error_When_Password_Too_Long()
  {
    var longPassword = new string('p', 101);
    var model = new SignUpCommand("user", longPassword, Role.Customer.ToString());
    var result = _validator.TestValidate(model);
    result.ShouldHaveValidationErrorFor(x => x.Password);
  }

  [Fact]
  public void Should_Have_Error_When_Role_Is_Invalid()
  {
    var model = new SignUpCommand("user", "password123", "999");
    var result = _validator.TestValidate(model);
    result.ShouldHaveValidationErrorFor(x => x.Role);
  }

  [Fact]
  public void Should_Not_Have_Error_When_Model_Is_Valid()
  {
    var model = new SignUpCommand("user", "password123", Role.Agent.ToString());
    var result = _validator.TestValidate(model);
    result.ShouldNotHaveAnyValidationErrors();
  }
}
