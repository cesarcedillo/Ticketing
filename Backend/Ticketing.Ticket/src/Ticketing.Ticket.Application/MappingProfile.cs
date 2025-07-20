using AutoMapper;
using Ticketing.Ticket.Application.Commands.AddTicketReply;
using Ticketing.Ticket.Application.Commands.CreateTicket;
using Ticketing.Ticket.Application.Dtos;
using Ticketing.Ticket.Application.Dtos.Requests;
using Ticketing.Ticket.Application.Dtos.Responses;
using Ticketing.Ticket.Domain.Aggregates;
using Ticketing.Ticket.Domain.Entities;
using TicketType = Ticketing.Ticket.Domain.Aggregates.Ticket;

namespace Ticketing.Ticket.Application;
public class MappingProfile : Profile
{
  public MappingProfile()
  {
    CreateMap<CreateTicketCommand, CreateTicketRequest>().ReverseMap();
    
    CreateMap<TicketType, CreateTicketResponse>().ReverseMap();

    CreateMap<TicketType, TicketResponse>()
        .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
        .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.User.Avatar));

    CreateMap<TicketType, TicketDetailResponse>()
        .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
        .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.User.Avatar))
        .ForMember(dest => dest.Replies, opt => opt.MapFrom(src => src.Replies.OrderBy(r => r.CreatedAt)));

    CreateMap<TicketReply, TicketReplyResponse>()
        .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
        .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.User.Avatar));

    CreateMap<AddTicketReplyRequest, AddTicketReplyCommand>()
        .ForMember(dest => dest.TicketId, opt => opt.Ignore());

    CreateMap<User, UserResponse>()
        .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.UserType.ToString()));

  }
}
