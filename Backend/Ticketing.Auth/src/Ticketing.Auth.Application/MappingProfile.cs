using AutoMapper;
using Ticketing.Auth.Application.Dtos.Responses;
using Ticketing.Auth.Domain.Aggregates;

namespace Ticketing.Auth.Application;
public class MappingProfile : Profile
{
  public MappingProfile()
  {
    CreateMap<User, UserResponse>()
        .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));
  }
}
