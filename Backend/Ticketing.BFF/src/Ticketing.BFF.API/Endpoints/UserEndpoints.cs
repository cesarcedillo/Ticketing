using Auth.Cliente.NswagAutoGen.HttpClientFactoryImplementation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Ticketing.BFF.Application.Commands.User.CreateUser;
using Ticketing.BFF.Application.Commands.User.Login;
using Ticketing.BFF.Application.Dto.Requests;
using Ticketing.BFF.Application.Dto.Responses;
using Ticketing.BFF.Application.Querires.User.GetUserByName;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

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
    group.MapPost("/signin", SignIn)
        .WithName("SignIn")
        .Produces<LoginResponseBff>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

    group.MapPost("/Me", Me)
        .WithName("Me")
        .RequireAuthorization()
        .Produces<UserResponseBff>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

    group.MapPost("", CreateUser)
        .WithName("CreateUser")
        .RequireAuthorization()
        .Produces<UserResponseBff>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

    return group;
  }

  public static async Task<IResult> SignIn(
      [FromBody] SingInRequest request,
      IMediator mediator,
      CancellationToken cancellationToken)
  {
    var command = new LoginCommandBff(request.Username!, request.Password!);
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

  public static async Task<IResult> Me(
      ClaimsPrincipal user,
      IMediator mediator,
      CancellationToken cancellationToken)
  {
    var username = user.Identity?.Name;
    if (string.IsNullOrEmpty(username))
      return Results.Unauthorized();

    var query = new GetUserByNameQueryBff(username);
    var result = await mediator.Send(query, cancellationToken);

    if (result is null)
      return Results.NotFound();

    return Results.Ok(result);
  }

  public static async Task<IResult> CreateUser(
      [FromBody] UserRequestBff userRequest,
      IMediator mediator,
      CancellationToken cancellationToken)
  {
    var command = new CreateUserCommandBff(userRequest.UserName, userRequest.Password, userRequest.Avatar, userRequest.Role);
    var result = await mediator.Send(command, cancellationToken);
    return Results.Ok(result);
  }
}
