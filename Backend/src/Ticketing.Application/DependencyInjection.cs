using Microsoft.Extensions.DependencyInjection;

namespace Ticketing.Application;
public static class DependencyInjection
{
  public static IServiceCollection AddApplicationServices(this IServiceCollection services)
  {
    //services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

    //services.AddMediatR(cfg =>
    //{
    //  cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    //  cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
    //  cfg.AddRequestPreProcessor(typeof(IRequestPreProcessor<>), typeof(LoggingBehaviour<>));
    //});

    //services.AddTransient<IMessengerSendService, MessengerSendServiceRabbit>();
    //services.AddTransient<IDacService, DacService>();
    //services.AddTransient<IMessengerService, MessengerService>();

    return services;
  }
}