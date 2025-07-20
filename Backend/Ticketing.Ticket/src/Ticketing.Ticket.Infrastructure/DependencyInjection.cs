using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ticketing.Ticket.Domain.Interfaces.Repositories;
using Ticketing.Ticket.Infrastructure.Data;
using Ticketing.Ticket.Infrastructure.Data.Repositories;

namespace Ticketing.Ticket.Infrastructure;

public static class DependencyInjection
{
  public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
  {
    var connectionString = configuration["RepositoryConnection"];
    if (string.IsNullOrEmpty(connectionString))
    {
      throw new Exception("The connection string is empty or null");
    }

    services.AddDbContext<TicketDbContext>(options => options.UseSqlite(connectionString));


    services.AddTransient<ITicketRepository, TicketRepository>();
    services.AddTransient<IUserRepository, UserRepository>();

    return services;
  }
}