using Auth.Cliente.NswagAutoGen.HttpClientFactoryImplementation;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Ticketing.BFF.Application.Commands.Ticket.AddTicketReply;
using Ticketing.BFF.Application.Commands.Ticket.CreateTicket;
using Ticketing.BFF.Application.Commands.Ticket.MarkTicketAsResolved;
using Ticketing.BFF.Application.Commands.User.Login;
using Ticketing.BFF.Application.Dto.Requests;
using Ticketing.BFF.Application.Dto.Responses;
using Ticketing.BFF.Application.Querires.Ticket.GetTicketDetail;
using Ticketing.BFF.Application.Querires.Ticket.ListTickets;
using Ticketing.BFF.Application.Querires.User.GetUserByName;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace Ticketing.BFF.API.Endpoints;
public static class TicketEndpoints
{
  public static void MapTicketEndpoints(this WebApplication app)
  {
    app.MapGroup("api/Ticket")
      .MapTicketPostEndpoints()
      .MapTicketGetEndpoints()
      .MapTicketPatchEndpoints();
  }

  public static RouteGroupBuilder MapTicketPostEndpoints(this RouteGroupBuilder group)
  {
    group.MapPost("/", CreateTicket)
        .WithName("CreateTicket")
        .RequireAuthorization()
        .Accepts<CreateTicketRequestBff>("application/json")
        .Produces<Guid>(StatusCodes.Status201Created)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

    group.MapPost("/{ticketId}/replies", AddTicketReply)
    .WithName("AddTicketReply")
        .RequireAuthorization()
    .Accepts<AddTicketReplyRequestBff>("application/json")
    .Produces(StatusCodes.Status204NoContent)
    .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
    .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);


    return group;
  }
  public static RouteGroupBuilder MapTicketGetEndpoints(this RouteGroupBuilder group)
  {
    group.MapGet("/", ListTickets)
    .WithName("ListTickets")
        .RequireAuthorization()
    .Produces<List<TicketResponseBff>>(StatusCodes.Status200OK)
    .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);


    group.MapGet("/{ticketId}", GetTicketDetail)
    .WithName("GetTicketDetail")
        .RequireAuthorization()
    .Produces<TicketDetailResponseBff>(StatusCodes.Status200OK)
    .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
    .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

    return group;
  }

  public static RouteGroupBuilder MapTicketPatchEndpoints(this RouteGroupBuilder group)
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
    var query = new ListTicketsQueryBff(status, userId);
    var tickets = await mediator.Send(query, cancellationToken);
    return Results.Ok(tickets);
  }

  public static async Task<IResult> GetTicketDetail(
    Guid ticketId,
    IMediator mediator,
    CancellationToken cancellationToken)
  {
    var query = new GetTicketDetailQueryBff(ticketId);
    var result = await mediator.Send(query, cancellationToken);

    return Results.Ok(result);
  }

  public static async Task<IResult> CreateTicket(
    [FromBody] CreateTicketRequestBff request,
    IMediator mediator,
    IMapper mapper,
    CancellationToken cancellationToken)
  {
    var command = mapper.Map<CreateTicketCommandBff>(request);
    var ticketId = await mediator.Send(command, cancellationToken);

    return Results.Created($"/api/Ticketing/{ticketId}", new { id = ticketId });
  }

  public static async Task<IResult> AddTicketReply(
    Guid ticketId,
    [FromBody] AddTicketReplyRequestBff request,
    IMediator mediator,
    IMapper mapper,
    CancellationToken cancellationToken)
  {
    var command = new AddTicketReplyCommandBff(ticketId, request.Text, request.UserId);

    await mediator.Send(command, cancellationToken);

    return Results.NoContent();
  }


  public static async Task<IResult> MarkTickedAsResolved(
    Guid ticketId,
    IMediator mediator,
    CancellationToken cancellationToken)
  {
    var command = new MarkTicketAsResolvedCommandBff(ticketId);

    await mediator.Send(command, cancellationToken);

    return Results.NoContent();
  }
}
