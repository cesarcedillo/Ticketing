using Microsoft.Extensions.DependencyInjection;
using Ticketing.BFF.Infrastructure.Http;

namespace Ticketing.BFF.Infrastructure;

public static class DependencyInjection
{
  public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
  {
    services.AddTransient<PropagateBearerTokenHandler>();

    return services;
  }
}