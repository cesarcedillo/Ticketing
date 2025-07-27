using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ticketing.Ticket.Application.Commands.AddTicketReply;
using Ticketing.Ticket.Application.Commands.CreateTicket;
using Ticketing.Ticket.Application.Commands.MarkTicketAsResolved;
using Ticketing.Ticket.Application.Dtos.Requests;
using Ticketing.Ticket.Application.Dtos.Responses;
using Ticketing.Ticket.Application.Queries.GetTicketDetail;
using Ticketing.Ticket.Application.Queries.ListTickets;

namespace Ticketing.Ticket.API.Endpoints;
public static class TicketEndpoints
{
  public static void MapTicketEndpoints(this WebApplication app)
  {
    app.MapGroup("api/Ticket")
      .MapTicketingPostEndpoints()
      .MapTicketingGetEndpoints()
      .MapTicketingPatchEndpoints();
  }
  public static RouteGroupBuilder MapTicketingPostEndpoints(this RouteGroupBuilder group)
  {
    group.MapPost("/", CreateTicket)
        .WithName("CreateTicket")
        .RequireAuthorization()
        .Accepts<CreateTicketRequest>("application/json")
        .Produces<Guid>(StatusCodes.Status201Created)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

    group.MapPost("/{ticketId}/replies", AddTicketReply)
    .WithName("AddTicketReply")
        .RequireAuthorization()
    .Accepts<AddTicketReplyRequest>("application/json")
    .Produces(StatusCodes.Status204NoContent)
    .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
    .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);


    return group;
  }
  public static RouteGroupBuilder MapTicketingGetEndpoints(this RouteGroupBuilder group)
  {
    group.MapGet("/", ListTickets)
    .WithName("ListTickets")
        .RequireAuthorization()
    .Produces<List<TicketResponse>>(StatusCodes.Status200OK)
    .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);


    group.MapGet("/{ticketId}", GetTicketDetail)
    .WithName("GetTicketDetail")
        .RequireAuthorization()
    .Produces<TicketDetailResponse>(StatusCodes.Status200OK)
    .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
    .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

    return group;
  }

  public static RouteGroupBuilder MapTicketingPatchEndpoints(this RouteGroupBuilder group)
  {
    group.MapPatch("/{ticketId}/mark-as-resolved", MarkTickedAsResolved)
    .WithName("MarkAsResolved")
        .RequireAuthorization()
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
    var query = new ListTicketsQuery(status, userId);
    var tickets = await mediator.Send(query, cancellationToken);
    return Results.Ok(tickets);
  }

  public static async Task<IResult> GetTicketDetail(
    Guid ticketId,
    IMediator mediator,
    CancellationToken cancellationToken)
  {
    var query = new GetTicketDetailQuery(ticketId);
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

  public static async Task<IResult> AddTicketReply(
    Guid ticketId,
    [FromBody] AddTicketReplyRequest request,
    IMediator mediator,
    IMapper mapper,
    CancellationToken cancellationToken)
  {
    var command = new AddTicketReplyCommand(ticketId, request.Text, request.UserId);

    await mediator.Send(command, cancellationToken);

    return Results.NoContent();
  }


  public static async Task<IResult> MarkTickedAsResolved(
    Guid ticketId,
    IMediator mediator,
    CancellationToken cancellationToken)
  {
    var command = new MarkTicketAsResolvedCommand(ticketId);

    await mediator.Send(command, cancellationToken);

    return Results.NoContent();
  }


}