using Auth.Cliente.NswagAutoGen.HttpClientFactoryImplementation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Ticketing.BFF.Application.Commands.User.Login;
using Ticketing.BFF.Application.Dto.Responses;
using Ticketing.BFF.Application.Querires.User;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;
using UserResponse = User.Cliente.NswagAutoGen.HttpClientFactoryImplementation.UserResponse;

namespace Ticketing.BFF.API.Endpoints;
public static class UserEndpoints
{
  public static void MapUserEndpoints(this WebApplication app)
  {
    app.MapGroup("api/User")
      .MapUserGetEndpoints()
      .MapUserPostEndpoints();
  }

  public static RouteGroupBuilder MapUserGetEndpoints(this RouteGroupBuilder group)
  {
    group.MapGet("/{userName}", GetUserByUserName)
        .WithName("GetUserByUserName")
        .RequireAuthorization()
        .Produces<UserResponseBff>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

    return group;
  }
  public static RouteGroupBuilder MapUserPostEndpoints(this RouteGroupBuilder group)
  {
    group.MapPost("/login", Login)
        .WithName("Login")
        .Produces<LoginResponseBff>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

    //group.MapPost("/Me", Me)
    //    .WithName("Me")
    //    .RequireAuthorization()
    //    .Produces<UserResponseBff>(StatusCodes.Status200OK)
    //    .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
    //    .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

    return group;
  }

  public static async Task<IResult> Login(
      [FromBody] LoginRequest request,
      IMediator mediator,
      CancellationToken cancellationToken)
  {
    var command = new LoginCommandBff(request.Username, request.Password);
    var result = await mediator.Send(command, cancellationToken);

    if (!result.Success)
      return Results.Unauthorized();

    return Results.Ok(result);
  }

  public static async Task<IResult> GetUserByUserName(
    string userName,
    IMediator mediator,
    CancellationToken cancellationToken)
  {
    var query = new GetUserByNameQueryBff(userName);
    var result = await mediator.Send(query, cancellationToken);

    return Results.Ok(result);
  }

  //public static async Task<IResult> Me(
  //    ClaimsPrincipal user,
  //    IMediator mediator,
  //    CancellationToken cancellationToken)
  //{
  //  var username = user.Identity?.Name;
  //  if (string.IsNullOrEmpty(username))
  //    return Results.Unauthorized();

  //  var query = new GetUserByNameQuery(username);
  //  var result = await mediator.Send(query, cancellationToken);

  //  if (result is null)
  //    return Results.NotFound();

  //  return Results.Ok(result);
  //}
}
