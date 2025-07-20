using Ticketing.Core.Domain.SeedWork.Interfaces;

namespace Ticketing.Auth.Application.Dtos;
public record UserDto(string UserName, string Role) : IDto;
