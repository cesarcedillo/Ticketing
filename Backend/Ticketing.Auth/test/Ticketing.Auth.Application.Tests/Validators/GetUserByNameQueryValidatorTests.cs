using FluentAssertions;
using Ticketing.Auth.Application.Queries.GetUserByName;

namespace Ticketing.Auth.Application.Tests.Validators;
public class GetUserByNameQueryValidatorTests
{
  private readonly GetUserByNameQueryValidator _validator = new();

  [Fact]
  public void Should_Have_Error_When_UserName_Is_Empty()
  {
    var query = new GetUserByNameQuery("");
    var result = _validator.Validate(query);

    result.Errors.Should().Contain(e => e.PropertyName == "UserName");
  }

  [Fact]
  public void Should_Have_Error_When_UserName_Exceeds_MaxLength()
  {
    var query = new GetUserByNameQuery(new string('a', 101));
    var result = _validator.Validate(query);

    result.Errors.Should().Contain(e => e.PropertyName == "UserName");
  }

  [Fact]
  public void Should_Not_Have_Error_For_Valid_UserName()
  {
    var query = new GetUserByNameQuery("validUser");
    var result = _validator.Validate(query);

    result.IsValid.Should().BeTrue();
  }
}
