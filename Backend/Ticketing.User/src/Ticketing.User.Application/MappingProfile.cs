using AutoMapper;
using Ticketing.User.Application.Dto.Responses;
using UserType = Ticketing.User.Domain.Aggregates.User;

namespace Ticketing.User.Application;
public class MappingProfile : Profile
{
  public MappingProfile()
  {
    CreateMap<UserType, UserResponse>()
        .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.UserType.ToString()));

  }
}