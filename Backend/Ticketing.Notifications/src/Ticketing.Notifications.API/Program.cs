using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net.Mime;
using System.Text.Json;
using Ticketing.Core.Observability.OpenTelemetry.Middleware;
using Ticketing.Notifications.API.Endpoints;
using Ticketing.Notifications.API.Extensions;
using Ticketing.Notifications.Infrastructure.Data;
using Ticketing.Notifications.Infrastructure.Extensions;

if (Environment.GetEnvironmentVariable("RunningInDocker") != "true")
{
  var root = Directory.GetCurrentDirectory();
  var envPath = Path.Combine(root, "..", "..", "..", "..", "Configuration", "notifications.env");
  DotNetEnv.Env.Load(envPath);
  envPath = Path.Combine(root, "..", "..", "..", "..", "Configuration", "authentication.env");
  DotNetEnv.Env.Load(envPath);
  envPath = Path.Combine(root, "..", "..", "..", "..", "Configuration", "brokerConfigurations.env");
  DotNetEnv.Env.Load(envPath);

}

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.RegisterServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();
}
else
{
  app.UseExceptionHandler("/Error");
  app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseExceptionHandler(opt => { });

app.UseOpenTelemetryPayload();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true));

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger(c =>
{
  c.RouteTemplate = "Notifications/swagger/{documentNAme}/swagger.json";
});

app.UseSwaggerUI(c =>
{
  c.SwaggerEndpoint("v1/swagger.json", "Notifications v1");
  c.RoutePrefix = "Notifications/swagger";
});

app.MapControllers();

app.MapNotificationsEndpoints();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

app.MapHealthChecks("/healthz/ready", new HealthCheckOptions
{
  Predicate = healthCheck => healthCheck.Tags.Contains("ready")
});

app.MapHealthChecks("/healthz/live", new HealthCheckOptions
{
  Predicate = _ => false,
  ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    }
});

app.MapHealthChecks("/healthcheck",
    new HealthCheckOptions
    {
      ResponseWriter = async (context, report) =>
      {
        var result = JsonSerializer.Serialize(
            new
            {
              status = report.Status.ToString(),
              errors = report.Entries.Select(e => new { key = e.Key, value = Enum.GetName(typeof(HealthStatus), e.Value.Status) })
            });
        context.Response.ContentType = MediaTypeNames.Application.Json;
        await context.Response.WriteAsync(result);
      }
    });

if (app.Environment.IsDevelopment() || app.Environment.IsIntegration())
{
  using (var scope = app.Services.CreateScope())
  {
    app.Services.ApplyMigrations();
    var db = scope.ServiceProvider.GetRequiredService<NotificationsDbContext>();
  }
}

app.Run();
