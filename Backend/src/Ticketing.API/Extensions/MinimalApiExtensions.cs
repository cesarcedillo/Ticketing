using AutoMapper;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using Ticketing.API.Extensions.Loggers.Configurations;
using Ticketing.API.Infrastructure;
using Ticketing.Application;
using Ticketing.Core.OpenTelemetry;
using Ticketing.Core.OpenTelemetry.Helpers;

namespace Ticketing.API.Extensions;

public static class MinimalApiExtensions
{
  public static void RegisterServices(this WebApplicationBuilder builder)
  {
    var Configuration = builder.Configuration;
    var Environment = builder.Environment;
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

    if (!Environment.IsTesting())
    {
      builder.Services.AddOpenTelemetryCustom(new OpenTelemetryOptions
                {
                  ServiceName = Configuration["ServiceName"] ?? "Unknown-Service",
                  PropertiesToTrace = Configuration.GetSection("PropertiesToTrace").Get<List<string>>() ?? [],
                  SamplingRatio = Configuration.GetValue<float>("SamplingRatio", 1.0F),
                  TraceContents = Configuration.GetValue<bool>("TraceContents", false)
                });
    }

    var mapperConfig = new MapperConfiguration(cfg =>
    {
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

