using Auth.Cliente.NswagAutoGen.HttpClientFactoryImplementation;
using AutoMapper;
using Ticketing.BFF.Application.Commands.User.Login;
using Ticketing.BFF.Application.Dto.Responses;

namespace Ticketing.BFF.Application;
public class MappingProfile : Profile
{
  public MappingProfile()
  {
    CreateMap<LoginCommandBff, LoginRequest>();
    CreateMap<LoginResponse, LoginResponseBff>();
    CreateMap<UserResponse, UserResponseBff>();
  }
}