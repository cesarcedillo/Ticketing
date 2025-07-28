using MediatR;
using Ticketing.User.Application.Dto.Responses;

namespace Ticketing.User.Application.Queries.GetUsersByIds;
public record GetUsersByIdsQuery(IEnumerable<Guid> UserIds) : IRequest<List<UserResponse>>;
