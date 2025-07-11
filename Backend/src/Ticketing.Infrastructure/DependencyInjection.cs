using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ticketing.Domain.Interfaces.Repositories;
using Ticketing.Infrastructure.Data;
using Ticketing.Infrastructure.Data.Repositories;

namespace Ticketing.Infrastructure;

public static class DependencyInjection
{
  public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
  {
    var connectionString = configuration["RepositoryConnection"];
    if (string.IsNullOrEmpty(connectionString))
    {
      throw new Exception("The connection string is empty or null");
    }

    //if (isDevelopment)
    //{
    //services.AddDbContext<TicketDbContext>(options => options.UseSqlite(connectionString));
    //}
    //else
    //{
    services.AddDbContext<TicketDbContext>(options => options.UseSqlServer(connectionString));
    //}


    services.AddTransient<ITicketRepository, TicketRepository>();

    return services;
  }
}