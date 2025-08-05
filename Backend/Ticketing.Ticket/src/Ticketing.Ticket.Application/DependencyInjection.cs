using FluentValidation;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Ticketing.Ticket.Application.Services;
using Ticketing.Ticket.Application.Services.Interfaces;
using Ticketing.Core.Application.Mediatr.Behaviours.Behaviours;
using Microsoft.Extensions.Configuration;
using Ticketing.Ticket.Application.Extensions;

namespace Ticketing.Ticket.Application;
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

    services.AddMessengerConfiguration(configuration);

    services.AddTransient<ITicketService, TicketService>();

    return services;
  }
}