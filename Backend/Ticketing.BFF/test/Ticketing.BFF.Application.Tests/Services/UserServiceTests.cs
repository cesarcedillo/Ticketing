using Auth.Cliente.NswagAutoGen.HttpClientFactoryImplementation;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Moq;
using Ticketing.BFF.Application.Dto.Responses;
using Ticketing.BFF.Application.Services;
using User.Cliente.NswagAutoGen.HttpClientFactoryImplementation;
using UserResponse = User.Cliente.NswagAutoGen.HttpClientFactoryImplementation.UserResponse;

namespace Ticketing.BFF.Tests.Application.Services
{
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

      // Simula el HttpContext para Items
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
      var loginRequest = new LoginRequest { Username = userName, Password = password };
      var loginResponse = new LoginResponse { Success = true, AccessToken = token, Message = "OK" };
      var mappedLoginResponseBff = new LoginResponseBff { Success = true, AccessToken = token, Message = "OK" };

      var user = new UserResponse() { Id = Guid.NewGuid(), UserName = userName, Avatar = "user_avatar", Type = "Admin"}; 
      var mappedUserBff = new UserResponseBff();

      _authClientMock
          .Setup(c => c.LoginAsync(It.Is<LoginRequest>(r => r.Username == userName && r.Password == password), It.IsAny<CancellationToken>()))
          .ReturnsAsync(loginResponse);

      _mapperMock
          .Setup(m => m.Map<LoginResponseBff>(loginResponse))
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

      // Verifica que se puso el token temporalmente y luego se eliminó
      Assert.False(_httpContext.Items.ContainsKey("ManualToken"));
    }

    [Fact]
    public async Task LoginAsync_WithInvalidCredentials_ShouldNotSetManualTokenOrCallUserClient()
    {
      // Arrange
      var userName = "admin";
      var password = "wrongpass";
      var loginResponse = new LoginResponse { Success = false, AccessToken = null, Message = "Bad credentials" };
      var mappedLoginResponseBff = new LoginResponseBff { Success = false, AccessToken = null, Message = "Bad credentials" };

      _authClientMock
          .Setup(c => c.LoginAsync(It.IsAny<LoginRequest>(), It.IsAny<CancellationToken>()))
          .ReturnsAsync(loginResponse);

      _mapperMock
          .Setup(m => m.Map<LoginResponseBff>(loginResponse))
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
  }
}
