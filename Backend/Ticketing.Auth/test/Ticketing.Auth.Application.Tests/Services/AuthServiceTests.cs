using FluentAssertions;
using Moq;
using Ticketing.Auth.Application.Dtos;
using Ticketing.Auth.Application.Services;
using Ticketing.Auth.Application.Services.Interfaces;
using Ticketing.Auth.Domain.Aggregates;
using Ticketing.Auth.Domain.Enums;
using Ticketing.Auth.Domain.Interfaces;

namespace Ticketing.Auth.Application.Tests.Services;
public class AuthServiceTests
{
  private readonly Mock<IUserRepository> _userRepoMock = new();
  private readonly Mock<IPasswordHasher> _passwordHasherMock = new();
  private readonly Mock<IJwtTokenGenerator> _jwtTokenGeneratorMock = new();

  private readonly AuthService _service;

  public AuthServiceTests()
  {
    _service = new AuthService(_userRepoMock.Object, _passwordHasherMock.Object, _jwtTokenGeneratorMock.Object);
  }

  [Fact]
  public async Task SignInAsync_Should_Return_Failure_When_User_Not_Found()
  {
    // Arrange
    _userRepoMock.Setup(x => x.GetByUserNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync((User)null);

    // Act
    var result = await _service.SignInAsync("nonexistent", "any", CancellationToken.None);

    // Assert
    result.Success.Should().BeFalse();
    result.Message.Should().Be("Invalid credentials");
  }

  [Fact]
  public async Task SignInAsync_Should_Return_Failure_When_Password_Invalid()
  {
    // Arrange
    var user = new User("user", "hash", Role.Customer);
    _userRepoMock.Setup(x => x.GetByUserNameAsync("user", It.IsAny<CancellationToken>()))
        .ReturnsAsync(user);
    _passwordHasherMock.Setup(x => x.Verify("wrongpass", "hash"))
        .Returns(false);

    // Act
    var result = await _service.SignInAsync("user", "wrongpass", CancellationToken.None);

    // Assert
    result.Success.Should().BeFalse();
    result.Message.Should().Be("Invalid credentials");
  }

  [Fact]
  public async Task SignInAsync_Should_Return_Success_When_Credentials_Valid()
  {
    // Arrange
    var user = new User("user", "hash", Role.Agent);
    var jwtTokenDto = new JwtTokenDto("the_token", DateTime.UtcNow.AddHours(2));
    _userRepoMock.Setup(x => x.GetByUserNameAsync("user", It.IsAny<CancellationToken>()))
        .ReturnsAsync(user);
    _passwordHasherMock.Setup(x => x.Verify("goodpass", "hash"))
        .Returns(true);

    _jwtTokenGeneratorMock.Setup(x => x.GenerateToken(user))
        .Returns(jwtTokenDto);

    // Act
    var result = await _service.SignInAsync("user", "goodpass", CancellationToken.None);

    // Assert
    result.Success.Should().BeTrue();
    result.AccessToken.Should().Be("the_token");
    result.UserName.Should().Be("user");
    result.Role.Should().Be("Agent");
  }

  [Fact]
  public async Task SignUpAsync_Should_Throw_When_User_Already_Exists()
  {
    // Arrange
    var existingUser = new User("existinguser", "hashed", Role.Admin);

    _userRepoMock.Setup(x => x.GetByUserNameAsync("existinguser", It.IsAny<CancellationToken>()))
        .ReturnsAsync(existingUser);

    // Act
    Func<Task> act = async () => await _service.SignUpAsync("existinguser", "password", Role.Admin, CancellationToken.None);

    // Assert
    await act.Should().ThrowAsync<InvalidOperationException>()
        .WithMessage("User with username 'existinguser' already exists.");
  }

  [Fact]
  public async Task SignUpAsync_Should_Create_User_And_Return_Token()
  {
    // Arrange
    var userName = "newuser";
    var password = "mypassword";
    var hashedPassword = "hashedpassword";
    var role = Role.Customer;
    var jwtTokenDto = new JwtTokenDto("signed-token", DateTime.UtcNow.AddHours(1));

    _userRepoMock.Setup(x => x.GetByUserNameAsync(userName, It.IsAny<CancellationToken>()))
        .ReturnsAsync((User)null);

    _passwordHasherMock.Setup(x => x.Hash(password))
        .Returns(hashedPassword);

    _userRepoMock.Setup(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
        .Returns(Task.CompletedTask);

    _jwtTokenGeneratorMock.Setup(x => x.GenerateToken(It.IsAny<User>()))
        .Returns(jwtTokenDto);

    // Act
    var result = await _service.SignUpAsync(userName, password, role, CancellationToken.None);

    // Assert
    result.Success.Should().BeTrue();
    result.AccessToken.Should().Be("signed-token");
    result.UserName.Should().Be(userName);
    result.Role.Should().Be(role.ToString());
    result.Expiration.Should().BeCloseTo(jwtTokenDto.Expiration, TimeSpan.FromSeconds(1));
  }

}
