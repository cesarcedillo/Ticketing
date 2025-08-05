using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ticketing.Notifications.Domain.Interfaces;
using Ticketing.Notifications.Infrastructure.Data;
using Ticketing.Notifications.Infrastructure.Data.Repositories;

namespace Ticketing.Notifications.Infrastructure;
public static class DependencyInjection
{
  public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
  {
    var connectionString = configuration["RepositoryConnection"];
    if (string.IsNullOrEmpty(connectionString))
    {
      throw new Exception("The connection string is empty or null");
    }

    services.AddDbContext<NotificationsDbContext>(options => options.UseSqlite(connectionString));

    services.AddTransient<INotificationRepository, NotificationRepository>();

    return services;
  }
}