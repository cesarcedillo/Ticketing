namespace Ticketing.API.Endpoints;
public static class TicketingEndpoints
{
  public static void MapTicketingEndpoints(this WebApplication app)
  {
    app.MapGroup("api/Ticketing")
      .MapTicketingPostEndpoints()
      .MapTicketingGetsEndpoints();
  }
  public static RouteGroupBuilder MapTicketingPostEndpoints(this RouteGroupBuilder group)
  {
    return group;
  }
  public static RouteGroupBuilder MapTicketingGetsEndpoints(this RouteGroupBuilder group)
  {
    return group;
  }
}