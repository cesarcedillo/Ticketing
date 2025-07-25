using AutoMapper;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Ticketing.BFF.API.Infrastructure;
using Ticketing.BFF.Application;

namespace Ticketing.BFF.API.Extensions;
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

    builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
      options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
      {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = Configuration["Issuer"],
        ValidAudience = Configuration["Audience"],
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
              System.Text.Encoding.UTF8.GetBytes(Configuration["Secret"] ?? "SOME_SECRET_KEY"))
      };
    });


    builder.Services.AddSwaggerGen(c =>
    {
      c.SwaggerDoc("v1",
          new OpenApiInfo
          {
            Title = "Ticketing.Bff",
            Version = "v1",
          });

      c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
      {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter the JWT token here. Example: Bearer {token}"
      });
      c.AddSecurityRequirement(new OpenApiSecurityRequirement
      {
          {
              new OpenApiSecurityScheme
              {
                  Reference = new OpenApiReference
                  {
                      Type = ReferenceType.SecurityScheme,
                      Id = "Bearer"
                  }
              },
              Array.Empty<string>()
          }
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

    builder.Services.AddAutoMapper(typeof(MappingProfile));
    IMapper mapper = mapperConfig.CreateMapper();
    builder.Services.AddSingleton(mapper);
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddExceptionHandler<CustomExceptionHandler>();
    builder.Services.AddApplicationServices(Configuration);
    builder.Services.AddSingleton(TimeProvider.System);
    builder.AddHealthChecks(Configuration);
  }
}