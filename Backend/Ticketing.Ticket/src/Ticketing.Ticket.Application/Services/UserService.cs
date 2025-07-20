using AutoMapper;
using Ticketing.Ticket.Application.Dtos.Responses;
using Ticketing.Ticket.Application.Services.Interfaces;
using Ticketing.Ticket.Domain.Interfaces.Repositories;

namespace Ticketing.Ticket.Application.Services;
public class UserService: IUserService
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
