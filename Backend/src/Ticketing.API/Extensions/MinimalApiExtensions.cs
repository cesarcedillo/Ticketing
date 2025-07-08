using AutoMapper;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using Ticketing.API.Extensions.Loggers.Configurations;
using Ticketing.API.Infrastructure;
using Ticketing.Application;

namespace Ticketing.API.Extensions;

public static class MinimalApiExtensions
{
  public static void RegisterServices(this WebApplicationBuilder builder)
  {
    var Configuration = builder.Configuration;
    var Environment = builder.Environment;
    // Add services to the container.
    builder.Services.AddControllers();

    if (Environment.IsDevelopment())
    {
      Configuration.AddUserSecrets(Assembly.GetExecutingAssembly());
    }

    builder.Host.UseSerilog((context, services, loggerConfiguration) =>
       loggerConfiguration
         .MinimumLevel.ControlledBy(new EnvironmentVariableLoggingLevelSwitch(Configuration["SerilogMinimumLevel"] ?? throw new ArgumentException("Minimum Serilog level not present, check CFG provider")))
         .WriteTo.Console()
     );

    builder.Services.AddCors(options =>
    {
      options.AddPolicy("PoliticaCors",
                builder =>
                {
                  builder.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
    });

    builder.Services.AddSwaggerGen(c =>
    {
      c.SwaggerDoc("v1",
                new OpenApiInfo
                {
                  Title = "Ticketing",
                  Version = "v1",
                });
    });

    var mapperConfig = new MapperConfiguration(cfg =>
    {
      //cfg.AddCollectionMappers();
      cfg.AddProfile<MappingProfile>();
    });

    Ticketing.Infrastructure.DependencyInjection.AddInfrastructureServices(builder.Services, Configuration, Environment.IsDevelopment());
    builder.Services.AddAutoMapper(typeof(MappingProfile));
    IMapper mapper = mapperConfig.CreateMapper();
    builder.Services.AddSingleton(mapper);
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddExceptionHandler<CustomExceptionHandler>();
    builder.Services.AddApplicationServices();
    builder.Services.AddSingleton(TimeProvider.System);
    builder.AddHealthChecks(Configuration);
  }
}

