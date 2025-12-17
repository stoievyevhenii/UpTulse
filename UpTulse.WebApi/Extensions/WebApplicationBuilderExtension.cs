using Quartz;

using UpTulse.Application;
using UpTulse.Application.Schedulers;
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

        public static WebApplicationBuilder ConfigureSchedulers(this WebApplicationBuilder builder)
        {
            builder.Services.AddQuartz(q =>
            {
                var jobKey = new JobKey("HistoryCleaner");
                q.AddJob<HistoryCleaner>(opts => opts.WithIdentity(jobKey));

                q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .StartNow()
                    .WithIdentity("HistoryCleaner-trigger")
                    .WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(10)
                        .RepeatForever())
                );
            });

            builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

            return builder;
        }
    }
}