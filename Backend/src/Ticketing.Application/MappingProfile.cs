using AutoMapper;
using Ticketing.Application.Commands.AddTicketReply;
using Ticketing.Application.Commands.CreateTicket;
using Ticketing.Application.Dtos;
using Ticketing.Application.Dtos.Requests;
using Ticketing.Application.Dtos.Responses;
using Ticketing.Domain.Aggregates;
using Ticketing.Domain.Entities;

namespace Ticketing.Application;
public class MappingProfile : Profile
{
  public MappingProfile()
  {
    CreateMap<CreateTicketCommand, CreateTicketRequest>().ReverseMap();
    
    CreateMap<Ticket, CreateTicketResponse>().ReverseMap();

    CreateMap<Ticket, TicketResponse>()
        .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
        .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.User.Avatar));

    CreateMap<Ticket, TicketDetailResponse>()
        .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
        .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.User.Avatar))
        .ForMember(dest => dest.Replies, opt => opt.MapFrom(src => src.Replies)); ;

    CreateMap<TicketReply, TicketReplyResponse>()
        .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
        .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.User.Avatar));

    CreateMap<AddTicketReplyRequest, AddTicketReplyCommand>()
        .ForMember(dest => dest.TicketId, opt => opt.Ignore());

    CreateMap<User, UserResponse>()
        .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.UserType.ToString()));

  }
}
