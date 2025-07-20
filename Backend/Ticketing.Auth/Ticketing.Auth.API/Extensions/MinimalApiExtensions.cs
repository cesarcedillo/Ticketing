using AutoMapper;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Ticketing.Auth.API.Infrastructure;
using Ticketing.Auth.Application;

namespace Ticketing.Auth.API.Extensions;

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
      builder.AddObservability(Configuration);
    }

    var mapperConfig = new MapperConfiguration(cfg =>
    {
      cfg.AddProfile<MappingProfile>();
    });

    Ticketing.Auth.Infrastructure.DependencyInjection.AddInfrastructureServices(builder.Services, Configuration, Environment.IsDevelopment());
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

