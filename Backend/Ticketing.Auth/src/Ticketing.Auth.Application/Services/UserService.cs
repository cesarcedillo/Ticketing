using AutoMapper;
using Ticketing.Auth.Application.Dtos.Responses;
using Ticketing.Auth.Application.Services.Interfaces;
using Ticketing.Auth.Domain.Interfaces;

namespace Ticketing.Auth.Application.Services;

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

    if (user == null)
      throw new KeyNotFoundException($"User {userName} not found.");

    return _mapper.Map<UserResponse>(user);
  }


}

