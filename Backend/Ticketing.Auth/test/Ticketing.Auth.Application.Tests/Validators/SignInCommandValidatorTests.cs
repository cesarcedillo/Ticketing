using FluentAssertions;
using Ticketing.Auth.Application.Commands.SignIn;

namespace Ticketing.Auth.Application.Tests.Validators;
public class SignInCommandValidatorTests
{
  private readonly SignInCommandValidator _validator = new();

  [Fact]
  public void Should_Have_Error_When_Username_Is_Empty()
  {
    var command = new SignInCommand("", "validPassword");
    var result = _validator.Validate(command);

    result.Errors.Should().Contain(e => e.PropertyName == "UserName");
  }

  [Fact]
  public void Should_Have_Error_When_Password_Is_Empty()
  {
    var command = new SignInCommand("validUser", "");
    var result = _validator.Validate(command);

    result.Errors.Should().Contain(e => e.PropertyName == "Password");
  }

  [Fact]
  public void Should_Have_Error_When_Password_Is_Too_Short()
  {
    var command = new SignInCommand("validUser", "123");
    var result = _validator.Validate(command);

    result.Errors.Should().Contain(e => e.PropertyName == "Password");
  }

  [Fact]
  public void Should_Have_Error_When_Username_Exceeds_MaxLength()
  {
    var command = new SignInCommand(new string('a', 101), "validPassword");
    var result = _validator.Validate(command);

    result.Errors.Should().Contain(e => e.PropertyName == "UserName");
  }

  [Fact]
  public void Should_Have_Error_When_Password_Exceeds_MaxLength()
  {
    var command = new SignInCommand("validUser", new string('b', 101));
    var result = _validator.Validate(command);

    result.Errors.Should().Contain(e => e.PropertyName == "Password");
  }

  [Fact]
  public void Should_Not_Have_Error_For_Valid_Command()
  {
    var command = new SignInCommand("validUser", "validPassword");
    var result = _validator.Validate(command);

    result.IsValid.Should().BeTrue();
  }
}
