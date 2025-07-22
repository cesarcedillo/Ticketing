using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ticketing.User.Application.Dto.Responses;
using Ticketing.User.Application.Queries.GetUserByName;

namespace Ticketing.User.API.Endpoints;
public static class UserEndpoints
{
  public static void MapUserEndpoints(this WebApplication app)
  {
    app.MapGroup("api/User")
      .MapUserGetEndpoints();
  }

  public static RouteGroupBuilder MapUserGetEndpoints(this RouteGroupBuilder group)
  {
    group.MapGet("/{userName}", GetUserByUserName)
    .WithName("GetUserByUserName")
    .Produces<UserResponse>(StatusCodes.Status200OK)
    .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
    .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

    return group;
  }

  public static async Task<IResult> GetUserByUserName(
    string userName,
    IMediator mediator,
    CancellationToken cancellationToken)
  {
    var query = new GetUserByNameQuery(userName);
    var result = await mediator.Send(query, cancellationToken);

    return Results.Ok(result);
  }
}