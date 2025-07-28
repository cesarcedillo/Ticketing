using FluentAssertions;
using Ticketing.User.Application.Queries.GetUsersByIds;

namespace Ticketing.User.Application.Tests.Validators;
public class GetUsersByIdsQueryValidatorTests
{
  private readonly GetUsersByIdsQueryValidator _validator;

  public GetUsersByIdsQueryValidatorTests()
  {
    _validator = new GetUsersByIdsQueryValidator();
  }

  [Fact]
  public void Should_Have_Error_When_UserIds_Is_Null()
  {
    // Arrange
    var query = new GetUsersByIdsQuery(null);

    // Act
    var result = _validator.Validate(query);

    // Assert
    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.PropertyName == "UserIds");
  }

  [Fact]
  public void Should_Have_Error_When_UserIds_Is_Empty()
  {
    // Arrange
    var query = new GetUsersByIdsQuery(Enumerable.Empty<Guid>());

    // Act
    var result = _validator.Validate(query);

    // Assert
    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.PropertyName == "UserIds");
  }

  [Fact]
  public void Should_Have_Error_When_UserIds_Contains_GuidEmpty()
  {
    // Arrange
    var ids = new[] { Guid.NewGuid(), Guid.Empty };
    var query = new GetUsersByIdsQuery(ids);

    // Act
    var result = _validator.Validate(query);

    // Assert
    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.PropertyName == "UserIds[1]");
  }

  [Fact]
  public void Should_Be_Valid_When_UserIds_Are_Valid()
  {
    // Arrange
    var ids = new[] { Guid.NewGuid(), Guid.NewGuid() };
    var query = new GetUsersByIdsQuery(ids);

    // Act
    var result = _validator.Validate(query);

    // Assert
    result.IsValid.Should().BeTrue();
  }
}