using AutoMapper;
using Ticketing.Application.Commands.CreateTicket;
using Ticketing.Application.Dtos;
using Ticketing.Application.Dtos.Requests;
using Ticketing.Domain.Aggregates;

namespace Ticketing.Application;
public class MappingProfile : Profile
{
  public MappingProfile()
  {
    CreateMap<CreateTicketCommand, CreateTicketRequest>().ReverseMap();
    CreateMap<Ticket, CreateTicketResponse>().ReverseMap();

  }
}
