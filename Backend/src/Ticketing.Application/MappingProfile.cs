using AutoMapper;
using Ticketing.Application.Dtos;
using Ticketing.Domain.Aggregates;

namespace Ticketing.Application;
public class MappingProfile : Profile
{
  public MappingProfile()
  {
    CreateMap<Ticket, GetTicketDto>().ReverseMap();

  }
}
