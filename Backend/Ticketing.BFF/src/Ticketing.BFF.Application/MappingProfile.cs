using Auth.Cliente.NswagAutoGen.HttpClientFactoryImplementation;
using UserResponse = User.Cliente.NswagAutoGen.HttpClientFactoryImplementation.UserResponse;
using AutoMapper;
using Ticketing.BFF.Application.Commands.User.Login;
using Ticketing.BFF.Application.Dto.Responses;

namespace Ticketing.BFF.Application;
public class MappingProfile : Profile
{
  public MappingProfile()
  {
    CreateMap<LoginCommandBff, LoginRequest>();
    CreateMap<LoginResponse, LoginResponseBff>()
        .ForMember(dest => dest.Expiration, opt =>
                                              opt.MapFrom(src => src.Expiration.HasValue ? 
                                                                src.Expiration.Value.UtcDateTime : 
                                                                (DateTime?)null));
    CreateMap<UserResponse, UserResponseBff>();
  }
}