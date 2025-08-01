using Auth.Cliente.NswagAutoGen.HttpClientFactoryImplementation;
using UserResponse = User.Cliente.NswagAutoGen.HttpClientFactoryImplementation.UserResponse;
using AutoMapper;
using Ticketing.BFF.Application.Commands.User.Login;
using Ticketing.BFF.Application.Dto.Responses;
using Ticket.Cliente.NswagAutoGen.HttpClientFactoryImplementation;
using Ticketing.BFF.Application.Dto.Requests;
using Ticketing.BFF.Application.Commands.Ticket.CreateTicket;

namespace Ticketing.BFF.Application;
public class MappingProfile : Profile
{
  public MappingProfile()
  {
    CreateMap<LoginCommandBff, SingInRequest>();
    CreateMap<CreateTicketRequestBff, CreateTicketCommandBff>();
    CreateMap<SignInResponse, LoginResponseBff>()
        .ForMember(dest => dest.Expiration, opt => opt.MapFrom(src => src.Expiration.HasValue ? 
                                                    src.Expiration.Value.UtcDateTime : 
                                                    (DateTime?)null));
    CreateMap<UserResponse, UserResponseBff>()
        .ForMember(dest => dest.Id, opt =>opt.MapFrom(src => src.Id!.Value.ToString()));

    CreateMap<TicketReplyResponse, TicketReplyResponseBff>()
      .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id!.Value))
      .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
      .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.HasValue ? 
                                                  src.CreatedAt.Value.UtcDateTime :
                                                  (DateTime?)null))
      .ForMember(dest => dest.User, opt => opt.Ignore());

    CreateMap<TicketResponse, TicketResponseBff>();


    CreateMap<TicketDetailResponse, TicketDetailResponseBff>()
      .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id!.Value))
      .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.Subject))
      .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
      .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
      .ForMember(dest => dest.User, opt => opt.Ignore())
      .ForMember(dest => dest.Replies, opt => opt.MapFrom(src => src.Replies ?? new List<TicketReplyResponse>()));

  }
}