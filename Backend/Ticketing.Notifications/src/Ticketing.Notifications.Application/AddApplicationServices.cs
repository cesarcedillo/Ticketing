using FluentValidation;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Ticketing.Core.Application.Mediatr.Behaviours.Behaviours;
using Ticketing.Notifications.Application.Services;
using Ticketing.Notifications.Application.Services.Interfaces;

namespace Ticketing.Notifications.Application;
public static class DependencyInjection
{
  public static IServiceCollection AddApplicationServices(this IServiceCollection services)
  {
    services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

    services.AddMediatR(cfg =>
    {
      cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
      cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
      cfg.AddRequestPreProcessor(typeof(IRequestPreProcessor<>), typeof(LoggingBehaviour<>));
    });

    services.AddTransient<INotificationsService, NotificationsService>();

    return services;
  }
}