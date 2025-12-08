using UpTulse.Application;
using UpTulse.DataAccess;
using UpTulse.DataAccess.Persistence;
using UpTulse.WebApi.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddProblemDetails()
    .AddDataAccessLayer(builder.Configuration)
    .AddApplicationLayer()
    .AddOpenApi()
    .AddHttpContextAccessor()
    .AddControllers(config => config.Filters.Add<ValidateModelAttribute>());

var app = builder.Build();

using var scope = app.Services
    .CreateScope();

await AutomatedMigration
    .MigrateAsync(scope.ServiceProvider);

app.UseCors(corsPolicyBuilder => corsPolicyBuilder
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(_ => true)
    .AllowCredentials()
    );

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();

namespace UpTulse.WebApi
{
    public partial class Program;
}