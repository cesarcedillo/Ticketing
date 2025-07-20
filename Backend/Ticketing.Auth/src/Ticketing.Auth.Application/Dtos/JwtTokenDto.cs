using Ticketing.Core.Domain.SeedWork.Interfaces;

namespace Ticketing.Auth.Application.Dtos;
public record JwtTokenDto(string Token, DateTime Expiration) : IDto;