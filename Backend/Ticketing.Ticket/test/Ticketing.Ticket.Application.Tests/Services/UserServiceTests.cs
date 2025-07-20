using AutoMapper;
using FluentAssertions;
using MockQueryable;
using Moq;
using Ticketing.Ticket.Application.Services;
using Ticketing.Ticket.Domain.Aggregates;
using Ticketing.Ticket.Domain.Enums;
using Ticketing.Ticket.Domain.Interfaces.Repositories;
using Ticketing.Ticket.TestCommon.Builders;
using Ticketing.Ticket.TestCommon.Fixtures;

namespace Ticketing.Ticket.Application.Tests.Services
{
  public class UserServiceTests
  {
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly IMapper _mapper;
    private readonly UserService _service;
    private readonly DomainFixture _fixture = new();

    public UserServiceTests()
    {
      _userRepositoryMock = new Mock<IUserRepository>();

      var config = new MapperConfiguration(cfg =>
      {
        cfg.AddProfile<MappingProfile>();
      });
      _mapper = config.CreateMapper();

      _service = new UserService(
          _userRepositoryMock.Object,
          _mapper
      );
    }
        
    [Fact]
    public async Task GetUserAsync_Should_Return_Dto_If_Exists()
    {
      // Arrange
      var user = _fixture.CreateDefaultCustomer();

      _userRepositoryMock.Setup(r => r.GetByUserNameAsync(user.UserName, It.IsAny<CancellationToken>()))
          .ReturnsAsync(user);

      // Act
      var result = await _service.GetUserByUserNameAsync(user.UserName, CancellationToken.None);

      // Assert
      result.Should().NotBeNull();
      result!.UserName.Should().Be(user.UserName);
    }

    [Fact]
    public async Task GetTicketDetailAsync_Should_Return_Null_If_Not_Found()
    {
      // Arrange
      User? user = null;
      var userName = "nonExistentUser";

      _userRepositoryMock.Setup(r => r.GetByUserNameAsync(userName, It.IsAny<CancellationToken>()))
          .ReturnsAsync(user);

      // Act
      var result = await _service.GetUserByUserNameAsync(userName, CancellationToken.None);

      // Assert
      result.Should().BeNull();
    }
  }
}
