using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Ticketing.Auth.Application.Commands.Login;
using Ticketing.Auth.Application.Dtos.Requests;
using Ticketing.Auth.Application.Dtos.Responses;
using Ticketing.Auth.Application.Queries.GetUserByName;

namespace Ticketing.Auth.API.Endpoints;

public static class AuthEndpoints
{
  public static void MapAuthEndpoints(this WebApplication app)
  {
    app.MapGroup("api/auth")
        .MapAuthPostEndpoints()
        .MapAuthGetEndpoints();
  }

  public static RouteGroupBuilder MapAuthPostEndpoints(this RouteGroupBuilder group)
  {
    group.MapPost("/login", Login)
        .WithName("Login")
        .Produces<LoginResponse>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

    return group;
  }

  public static RouteGroupBuilder MapAuthGetEndpoints(this RouteGroupBuilder group)
  {
    group.MapGet("/me", Me)
        .WithName("Me")
        .RequireAuthorization()
        .Produces<UserResponse>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

    return group;
  }

  public static async Task<IResult> Login(
      [FromBody] LoginRequest request,
      IMediator mediator,
      CancellationToken cancellationToken)
  {
    var command = new LoginCommand(request.Username, request.Password);
    var result = await mediator.Send(command, cancellationToken);

    if (!result.Success)
      return Results.Unauthorized();

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

    var query = new GetUserByNameQuery(username);
    var result = await mediator.Send(query, cancellationToken);

    if (result is null)
      return Results.NotFound();

    return Results.Ok(result);
  }
}

