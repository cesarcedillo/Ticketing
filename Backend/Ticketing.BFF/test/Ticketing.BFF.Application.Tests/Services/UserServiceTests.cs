using Auth.Cliente.NswagAutoGen.HttpClientFactoryImplementation;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Moq;
using Newtonsoft.Json.Linq;
using Ticketing.BFF.Application.Dto.Responses;
using Ticketing.BFF.Application.Services;
using User.Cliente.NswagAutoGen.HttpClientFactoryImplementation;
using UserResponse = User.Cliente.NswagAutoGen.HttpClientFactoryImplementation.UserResponse;

namespace Ticketing.BFF.Tests.Application.Services;
public class UserServiceTests
{
  private readonly Mock<IAuthClient> _authClientMock;
  private readonly Mock<IUserClient> _userClientMock;
  private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
  private readonly Mock<IMapper> _mapperMock;
  private readonly UserService _service;
  private readonly DefaultHttpContext _httpContext;

  public UserServiceTests()
  {
    _authClientMock = new Mock<IAuthClient>();
    _userClientMock = new Mock<IUserClient>();
    _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
    _mapperMock = new Mock<IMapper>();

    _httpContext = new DefaultHttpContext();
    _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(_httpContext);

    _service = new UserService(
        _authClientMock.Object,
        _userClientMock.Object,
        _httpContextAccessorMock.Object,
        _mapperMock.Object
    );
  }

  [Fact]
  public async Task LoginAsync_WithValidCredentials_ShouldSetManualTokenAndReturnUser()
  {
    // Arrange
    var userName = "admin";
    var password = "1234";
    var token = "my_token";
    var signInResponse = new SignInResponse { Success = true, AccessToken = token, Message = "OK" };
    var mappedLoginResponseBff = new LoginResponseBff { Success = true, AccessToken = token, Message = "OK" };

    var user = new UserResponse() { Id = Guid.NewGuid(), UserName = userName, Avatar = "user_avatar", Type = "Admin" };
    var mappedUserBff = new UserResponseBff();

    _authClientMock
        .Setup(c => c.SignInAsync(It.Is<SingInRequest>(r => r.Username == userName && r.Password == password), It.IsAny<CancellationToken>()))
        .ReturnsAsync(signInResponse);

    _mapperMock
        .Setup(m => m.Map<LoginResponseBff>(signInResponse))
        .Returns(mappedLoginResponseBff);

    _userClientMock
        .Setup(c => c.GetUserByUserNameAsync(userName, It.IsAny<CancellationToken>()))
        .ReturnsAsync(user);

    _mapperMock
        .Setup(m => m.Map<UserResponseBff>(user))
        .Returns(mappedUserBff);

    // Act
    var result = await _service.LoginAsync(userName, password, CancellationToken.None);

    // Assert
    Assert.True(result.Success);
    Assert.Equal(token, result.AccessToken);
    Assert.Equal(mappedUserBff, result.User);

    Assert.False(_httpContext.Items.ContainsKey("ManualToken"));
  }

  [Fact]
  public async Task LoginAsync_WithInvalidCredentials_ShouldNotSetManualTokenOrCallUserClient()
  {
    // Arrange
    var userName = "admin";
    var password = "wrongpass";
    var signInResponse = new SignInResponse { Success = false, AccessToken = null, Message = "Bad credentials" };
    var mappedLoginResponseBff = new LoginResponseBff { Success = false, AccessToken = null, Message = "Bad credentials" };

    _authClientMock
        .Setup(c => c.SignInAsync(It.IsAny<SingInRequest>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(signInResponse);

    _mapperMock
        .Setup(m => m.Map<LoginResponseBff>(signInResponse))
        .Returns(mappedLoginResponseBff);

    // Act
    var result = await _service.LoginAsync(userName, password, CancellationToken.None);

    // Assert
    Assert.False(result.Success);
    Assert.Null(result.AccessToken);
    Assert.Null(result.User);
    Assert.False(_httpContext.Items.ContainsKey("ManualToken"));
    _userClientMock.Verify(c => c.GetUserByUserNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
  }

  [Fact]
  public async Task GetUserByUserNameAsync_ShouldReturnMappedUser()
  {
    // Arrange
    var userName = "testuser";
    var user = new UserResponse { UserName = userName };
    var mappedUserBff = new UserResponseBff { UserName = userName };

    _userClientMock
        .Setup(c => c.GetUserByUserNameAsync(userName, It.IsAny<CancellationToken>()))
        .ReturnsAsync(user);

    _mapperMock
        .Setup(m => m.Map<UserResponseBff>(user))
        .Returns(mappedUserBff);

    // Act
    var result = await _service.GetUserByUserNameAsync(userName, CancellationToken.None);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(userName, result.UserName);
  }

  [Fact]
  public async Task CreateUserAsync_ShouldCreateUserAndCredentialsSuccessfully()
  {
    // Arrange
    var userName = "admin";
    var password = "1234";
    var avatar = "avatar.png";
    var role = "Admin";
    var token = "my_token";

    var signInResponse = new SignInResponse { Success = true, AccessToken = token, Message = "OK" };

    var createdUser = new UserResponse
    {
      Id = Guid.NewGuid(),
      UserName = userName,
      Avatar = avatar,
      Type = role
    };

    _userClientMock
        .Setup(c => c.CreateUserAsync(It.IsAny<UserRequest>(),It.IsAny<CancellationToken>()))
        .ReturnsAsync(createdUser);

    _authClientMock
        .Setup(c => c.SignUpAsync(It.IsAny<SingUpRequest>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(signInResponse);

    // Act
    var result = await _service.CreateUserAsync(userName, password, avatar, role, CancellationToken.None);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(createdUser.Id.ToString(), result.Id);
    Assert.Equal(createdUser.UserName, result.UserName);
    Assert.Equal(createdUser.Avatar, result.Avatar);
    Assert.Equal(createdUser.Type, result.Type);

    _userClientMock.Verify(c => c.CreateUserAsync(It.IsAny<UserRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    _authClientMock.Verify(c => c.SignUpAsync(It.IsAny<SingUpRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    _userClientMock.Verify(c => c.DeleteUserAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
  }

  [Fact]
  public async Task CreateUserAsync_WhenAuthFails_ShouldDeleteUserAndThrow()
  {
    // Arrange
    var userName = "testuser";
    var password = "testpass";
    var avatar = "avatar.png";
    var role = "admin";

    var createdUser = new UserResponse
    {
      Id = Guid.NewGuid(),
      UserName = userName,
      Avatar = avatar,
      Type = role
    };

    _userClientMock
        .Setup(c => c.CreateUserAsync(It.IsAny<UserRequest>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(createdUser);

    _authClientMock
        .Setup(c => c.SignUpAsync(It.IsAny<SingUpRequest>(), It.IsAny<CancellationToken>()))
        .ThrowsAsync(new Exception("Auth service failure"));

    _userClientMock
        .Setup(c => c.DeleteUserAsync(userName, It.IsAny<CancellationToken>()))
        .Returns(Task.CompletedTask);

    // Act & Assert
    var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
        _service.CreateUserAsync(userName, password, avatar, role, CancellationToken.None));

    Assert.Equal("Error creating credentials. User rolled back.", ex.Message);
    Assert.NotNull(ex.InnerException);
    Assert.Equal("Auth service failure", ex.InnerException!.Message);

    _userClientMock.Verify(c => c.DeleteUserAsync(userName, It.IsAny<CancellationToken>()), Times.Once);
  }

}

