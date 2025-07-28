using AutoMapper;
using MediatR;
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

    if (user == null)
      throw new KeyNotFoundException($"User {userName} not found.");

    var userResponse = _mapper.Map<UserResponse>(user);

    return userResponse;
  }

  public async Task<IEnumerable<UserResponse>> GetUsersByIdsAsync(IEnumerable<Guid> userIds, CancellationToken cancellationToken)
  {
    var users = await _userRepository.GetByIdsAsync(userIds, cancellationToken);
    return users.Select(u => _mapper.Map<UserResponse>(u));
  }


}
