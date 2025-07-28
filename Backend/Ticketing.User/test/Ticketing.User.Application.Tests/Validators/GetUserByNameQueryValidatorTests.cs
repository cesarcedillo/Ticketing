using FluentAssertions;
using Ticketing.User.Application.Queries.GetUserByName;

namespace Ticketing.User.Application.Tests.Validators;
public class GetUserByNameQueryValidatorTests
{
  private readonly GetUserByNameQueryValidator _validator = new();

  [Fact]
  public void Should_Have_Error_When_UserName_Is_Null()
  {
    // Arrange
    var query = new GetUserByNameQuery(null);

    // Act
    var result = _validator.Validate(query);

    // Assert
    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.PropertyName == "UserName");
  }

  [Fact]
  public void Should_Have_Error_When_UserName_Is_Empty()
  {
    // Arrange
    var query = new GetUserByNameQuery(string.Empty);

    // Act
    var result = _validator.Validate(query);

    // Assert
    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.PropertyName == "UserName");
  }

  [Fact]
  public void Should_Be_Valid_When_UserName_Is_NotEmpty()
  {
    // Arrange
    var query = new GetUserByNameQuery("alice");

    // Act
    var result = _validator.Validate(query);

    // Assert
    result.IsValid.Should().BeTrue();
  }
}
