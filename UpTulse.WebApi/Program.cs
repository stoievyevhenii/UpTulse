using UpTulse.DataAccess.Persistence;
using UpTulse.WebApi;
using UpTulse.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder
    .ConfigureBaseComponents()
    .ConfigureProjectLayers();

builder.Services.AddJwtAuthViaBearer(builder.Configuration);

var app = builder.Build();

using var scope = app.Services.CreateScope();
await AutomatedMigration.MigrateAsync(scope.ServiceProvider);

app
    .ConfigureSecurity()
    .ConfigureBaseMiddlewares()
    .ConfigureDebugInstruments();

await app.RunAsync();

namespace UpTulse.WebApi
{
    public partial class Program;
}