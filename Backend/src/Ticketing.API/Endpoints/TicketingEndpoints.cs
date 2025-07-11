using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ticketing.Application.Commands.CreateTicket;
using Ticketing.Application.Commands.MarkTicketAsResolved;
using Ticketing.Application.Dtos.Requests;
using Ticketing.Application.Dtos.Responses;
using Ticketing.Application.Queries.GetTicketDetail;
using Ticketing.Application.Queries.ListTickets;

namespace Ticketing.API.Endpoints;
public static class TicketingEndpoints
{
  public static void MapTicketingEndpoints(this WebApplication app)
  {
    app.MapGroup("api/Ticketing")
      .MapTicketingPostEndpoints()
      .MapTicketingGetEndpoints()
      .MapTicketingPatchEndpoints();
  }
  public static RouteGroupBuilder MapTicketingPostEndpoints(this RouteGroupBuilder group)
  {
    group.MapPost("/", CreateTicket)
        .WithName("CreateTicket")
        .Accepts<CreateTicketRequest>("application/json")
        .Produces<Guid>(StatusCodes.Status201Created)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

    return group;
  }
  public static RouteGroupBuilder MapTicketingGetEndpoints(this RouteGroupBuilder group)
  {
    group.MapGet("/", ListTickets)
    .WithName("ListTickets")
    .Produces<List<TicketResponse>>(StatusCodes.Status200OK)
    .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);


    group.MapGet("/{ticketId}", GetTicketDetail)
    .WithName("GetTicketDetail")
    .Produces<TicketDetailResponse>(StatusCodes.Status200OK)
    .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
    .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

    return group;
  }

  public static RouteGroupBuilder MapTicketingPatchEndpoints(this RouteGroupBuilder group)
  {
    group.MapPatch("/{ticketId}/mark-as-resolved", MarkTickedAsResolved)
    .WithName("MarkAsResolved")
    .Produces(StatusCodes.Status204NoContent)
    .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
    .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);

    return group;
  }

  public static async Task<IResult> ListTickets(
    [FromQuery] string? status,
    [FromQuery] Guid? userId,
    IMediator mediator,
    CancellationToken cancellationToken)
  {
    var query = new ListTicketsQuery
    {
      Status = status,
      UserId = userId
    };
    var tickets = await mediator.Send(query, cancellationToken);
    return Results.Ok(tickets);
  }

  public static async Task<IResult> GetTicketDetail(
    Guid ticketId,
    IMediator mediator,
    CancellationToken cancellationToken)
  {
    var query = new GetTicketDetailQuery { TicketId = ticketId };
    var result = await mediator.Send(query, cancellationToken);

    if (result == null)
      return Results.NotFound(new ProblemDetails
      {
        Title = "Ticket not found",
        Detail = $"Ticket with id {ticketId} does not exist."
      });

    return Results.Ok(result);
  }

  public static async Task<IResult> CreateTicket(
    [FromBody] CreateTicketRequest request,
    IMediator mediator,
    IMapper mapper,
    CancellationToken cancellationToken)
  {
    var command = mapper.Map<CreateTicketCommand>(request);
    var ticketId = await mediator.Send(command, cancellationToken);

    return Results.Created($"/api/Ticketing/{ticketId}", new { id = ticketId });
  }

  public static async Task<IResult> MarkTickedAsResolved(
    Guid ticketId,
    IMediator mediator,
    CancellationToken cancellationToken)
  {
    var command = new MarkTicketAsResolvedCommand
    {
      TicketId = ticketId,
    };

    await mediator.Send(command, cancellationToken);

    return Results.NoContent();
  }


}