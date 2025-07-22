using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ticketing.User.Domain.Interfaces.Repositories;
using Ticketing.User.Infraestructure.Data;
using Ticketing.User.Infraestructure.Data.Repositories;

namespace Ticketing.User.Infraestructure;
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