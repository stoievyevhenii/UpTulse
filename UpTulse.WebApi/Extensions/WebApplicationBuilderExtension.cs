using Quartz;

using UpTulse.Application;
using UpTulse.DataAccess;
using UpTulse.WebApi.BackgroundWorkers;
using UpTulse.WebApi.Filters;

namespace UpTulse.WebApi.Extensions
{
    public static class WebApplicationBuilderExtension
    {
        public static WebApplicationBuilder ConfigureBackgroundServices(this WebApplicationBuilder builder)
        {
            return builder;
        }

        public static WebApplicationBuilder ConfigureBaseComponents(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers(config => config.Filters.Add<ValidateModelAttribute>());
            builder.Services.AddProblemDetails();
            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddHostedService<UptimeBackgroundWorker>();

            return builder;
        }

        public static WebApplicationBuilder ConfigureProjectLayers(this WebApplicationBuilder builder)
        {
            builder.Services
                .AddDataAccessLayer(builder.Configuration)
                .AddApplicationLayer();

            return builder;
        }
    }
}