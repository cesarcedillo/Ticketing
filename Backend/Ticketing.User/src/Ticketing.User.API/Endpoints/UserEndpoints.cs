using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ticketing.User.Application.Commands.CreateUser;
using Ticketing.User.Application.Commands.DeleteUser;
using Ticketing.User.Application.Dto.Requests;
using Ticketing.User.Application.Dto.Responses;
using Ticketing.User.Application.Queries.GetUserByName;
using Ticketing.User.Application.Queries.GetUsersByIds;

namespace Ticketing.User.API.Endpoints;
public static class UserEndpoints
{
  public static void MapUserEndpoints(this WebApplication app)
  {
    app.MapGroup("api/User")
      .MapUserGetEndpoints()
      .MapUserPostEndpoints()
      .MapUserDeleteEndpoints();
  }

  public static RouteGroupBuilder MapUserGetEndpoints(this RouteGroupBuilder group)
  {
    group.MapGet("/{userName}", GetUserByUserName)
    .WithName("GetUserByUserName")
        .RequireAuthorization()
        .Produces<UserResponse>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

    group.MapPost("/by-ids", GetUsersByIds)
        .WithName("GetUsersByIds")
        .RequireAuthorization()
        .Produces<List<UserResponse>>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

    return group;
  }

  public static RouteGroupBuilder MapUserPostEndpoints(this RouteGroupBuilder group)
  {
    group.MapPost("", CreateUser)
    .WithName("CreateUser")
        .RequireAuthorization()
        .Produces<UserResponse>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

    return group;
  }

  public static RouteGroupBuilder MapUserDeleteEndpoints(this RouteGroupBuilder group)
  {
    group.MapPost("{userName}", DeleteUser)
    .WithName("DeleteUser")
        .RequireAuthorization()
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
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

  public static async Task<IResult> GetUsersByIds(
    [FromBody] IEnumerable<Guid> userIds,
    IMediator mediator,
    CancellationToken cancellationToken)
  {
    var query = new GetUsersByIdsQuery(userIds);
    var result = await mediator.Send(query, cancellationToken);

    return Results.Ok(result);
  }

  public static async Task<IResult> CreateUser(
      [FromBody] UserRequest userRequest,
      IMediator mediator,
      CancellationToken cancellationToken)
  {
    var command = new CreateUserCommand(userRequest.UserName, userRequest.Avatar, userRequest.Role);
    var result = await mediator.Send(command, cancellationToken);
    return Results.Ok(result);
  }

  private static async Task<IResult> DeleteUser(string userName, 
                                                  IMediator mediator, 
                                                  CancellationToken cancellationToken)
  {
    await mediator.Send(new DeleteUserCommand(userName), cancellationToken);
    return Results.NoContent();
  }


}