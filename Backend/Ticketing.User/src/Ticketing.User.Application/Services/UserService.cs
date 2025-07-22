using AutoMapper;
using Ticketing.User.Application.Dto.Responses;
using Ticketing.User.Application.Services.Interfaces;
using Ticketing.User.Domain.Interfaces.Repositories;

namespace Ticketing.User.Application.Services;
public class UserService : IUserService
{
  private readonly IUserRepository _userRepository;
  private readonly IMapper _mapper;

  public UserService(IUserRepository userRepository, IMapper mapper)
  {
    _userRepository = userRepository;
    _mapper = mapper;
  }

  public async Task<UserResponse> GetUserByUserNameAsync(string userName, CancellationToken cancellationToken)
  {
    var user = await _userRepository.GetByUserNameAsync(userName, cancellationToken);

    var userResponse = _mapper.Map<UserResponse>(user);

    return userResponse;
  }

}
