using AutoMapper;
using Ticketing.Application.Dtos.Responses;
using Ticketing.Domain.Interfaces.Repositories;

namespace Ticketing.Application.Services;
public class UserService
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
