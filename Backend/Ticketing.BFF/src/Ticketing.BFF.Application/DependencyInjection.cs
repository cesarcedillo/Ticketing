using Auth.Cliente.NswagAutoGen.HttpClientFactoryImplementation;
using FluentValidation;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Ticketing.BFF.Application.Services.Interfaces;
using Ticketing.BFF.Application.Services;
using Ticketing.BFF.Infrastructure.Http;
using Ticketing.Core.Application.Mediatr.Behaviours.Behaviours;
using User.Cliente.NswagAutoGen.HttpClientFactoryImplementation;

namespace Ticketing.BFF.Application;
public static class DependencyInjection
{
  public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());



    services.AddMediatR(cfg =>
    {
      cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
      cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
      cfg.AddRequestPreProcessor(typeof(IRequestPreProcessor<>), typeof(LoggingBehaviour<>));
    });

    services.AddTransient<IUserService, UserService>();

    services.AddHttpContextAccessor();

    services.AddScoped<IAuthClient, AuthClient>();
    services.AddHttpClient(
                    nameof(AuthClient),
                    configureClient => configureClient.BaseAddress = new Uri(configuration["AuthClientUrl"] ??
                    throw new Exception("AuthClientUrl not exits in the configuration")))
            .AddHttpMessageHandler<PropagateBearerTokenHandler>();

    services.AddScoped<IUserClient, UserClient>();
    services.AddHttpClient(
                    nameof(UserClient),
                    configureClient => configureClient.BaseAddress = new Uri(configuration["UserClientUrl"] ??
                    throw new Exception("UserClientUrl not exits in the configuration")))
            .AddHttpMessageHandler<PropagateBearerTokenHandler>(); ;

    return services;
  }
}