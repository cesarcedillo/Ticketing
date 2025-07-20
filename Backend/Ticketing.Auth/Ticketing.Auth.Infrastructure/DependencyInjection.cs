using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ticketing.Auth.Domain.Interfaces;
using Ticketing.Auth.Infrastructure.Data;
using Ticketing.Auth.Infrastructure.Data.Repositories;
using Ticketing.Auth.Infrastructure.Services;

namespace Ticketing.Auth.Infrastructure;

public static class DependencyInjection
{
  public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
  {
    var connectionString = configuration["RepositoryConnection"];
    if (string.IsNullOrEmpty(connectionString))
    {
      throw new Exception("The connection string is empty or null");
    }

    services.AddDbContext<AuthDbContext>(options => options.UseSqlite(connectionString));

    services.AddScoped<IPasswordHasher, SimplePasswordHasher>();
    services.AddTransient<IUserRepository, UserRepository>();

    return services;
  }
}