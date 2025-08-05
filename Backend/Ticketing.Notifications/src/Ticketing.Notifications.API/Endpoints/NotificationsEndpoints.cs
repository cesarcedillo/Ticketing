namespace Ticketing.Notifications.API.Endpoints;
public static class UserEndpoints
{
  public static void MapNotificationsEndpoints(this WebApplication app)
  {
    app.MapGroup("api/notifications")
      .MapNotificationsGetEndpoints();
  }

  public static RouteGroupBuilder MapNotificationsGetEndpoints(this RouteGroupBuilder group)
  {
    return group;
  }
}