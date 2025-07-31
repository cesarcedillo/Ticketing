using AutoMapper;
using Ticketing.User.Application.Dto.Responses;
using Ticketing.User.Application.Services.Interfaces;
using Ticketing.User.Domain.Enums;
using Ticketing.User.Domain.Interfaces.Repositories;
using UserType = Ticketing.User.Domain.Aggregates.User;

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

  public async Task<UserResponse> CreateUserAsync(string userName, string avatar, string role, CancellationToken cancellationToken)
  {
    var existingUser = await _userRepository.GetByUserNameAsync(userName, cancellationToken);
    if (existingUser != null)
      throw new InvalidOperationException($"User with username '{userName}' already exists.");

    if (!Enum.TryParse<Role>(role, true, out var roleEnum))
      throw new ArgumentException($"Role '{role}' is not valid.", nameof(role));

    var user = new UserType(userName, avatar, roleEnum);

    await _userRepository.AddAsync(user, cancellationToken);

    var userResponse = _mapper.Map<UserResponse>(user);

    return userResponse;
  }

  public async Task DeleteUserAsync(string userName, CancellationToken cancellationToken)
  {
    var user = await _userRepository.GetByUserNameAsync(userName, cancellationToken);

    if (user == null)
      throw new KeyNotFoundException($"User '{userName}' not found.");

    await _userRepository.DeleteAsync(user.Id, cancellationToken);
  }


}
