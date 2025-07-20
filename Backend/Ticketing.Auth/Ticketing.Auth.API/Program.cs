using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net.Mime;
using System.Text.Json;
using Ticketing.Auth.API.Endpoints;
using Ticketing.Auth.API.Extensions;
using Ticketing.Auth.Infrastructure.Data;
using Ticketing.Auth.Infrastructure.Extensions;
using Ticketing.Core.Observability.OpenTelemetry.Middleware;

var builder = WebApplication.CreateBuilder(args);
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

app.UseAuthorization();

app.UseSwagger(c =>
{
  c.RouteTemplate = "Auth/swagger/{documentNAme}/swagger.json";
});

app.UseSwaggerUI(c =>
{
  c.SwaggerEndpoint("v1/swagger.json", "Auth v1");
  c.RoutePrefix = "Auth/swagger";
});

app.MapControllers();

app.MapAuthEndpoints();

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
    var db = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
    await DbSeeder.SeedAsync(db);
  }
}

app.Run();
