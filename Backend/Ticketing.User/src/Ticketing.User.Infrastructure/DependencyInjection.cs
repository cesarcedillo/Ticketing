using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ticketing.User.Domain.Interfaces.Repositories;
using Ticketing.User.infrastructure.Data;
using Ticketing.User.infrastructure.Data.Repositories;

namespace Ticketing.User.infrastructure;
public static class DependencyInjection
{
  public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
  {
    var connectionString = configuration["RepositoryConnection"];
    if (string.IsNullOrEmpty(connectionString))
    {
      throw new Exception("The connection string is empty or null");
    }

    services.AddDbContext<UserDbContext>(options => options.UseSqlite(connectionString));

    services.AddTransient<IUserRepository, UserRepository>();

    return services;
  }
}