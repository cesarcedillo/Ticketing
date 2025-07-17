using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Ticketing.API.Endpoints;
using Ticketing.API.Extensions;
using Ticketing.Infrastructure.Data;
using Ticketing.Infrastructure.Extensions;


var builder = WebApplication.CreateBuilder(args);
builder.RegisterServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
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

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true));

app.UseAuthorization();

app.UseSwagger(c =>
{
  c.RouteTemplate = "Ticketing/swagger/{documentNAme}/swagger.json";
});

app.UseSwaggerUI(c =>
{
  c.SwaggerEndpoint("v1/swagger.json", "Ticketing v1");
  c.RoutePrefix = "Ticketing/swagger";
});

app.MapControllers();

app.MapTicketEndpoints();
app.MapUserEndpoints();

app.UseExceptionHandler(opt => { });

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
    var db = scope.ServiceProvider.GetRequiredService<TicketDbContext>();
    await DbSeeder.SeedAsync(db);
  }
}

app.Run();
