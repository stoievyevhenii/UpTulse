using FluentValidation;

using Microsoft.Extensions.DependencyInjection;

using UpTulse.Application.MapperConfigs;
using UpTulse.Application.Models;
using UpTulse.Application.ModelsValidators;
using UpTulse.Application.Services;
using UpTulse.Application.Services.Impl;
using UpTulse.Shared.Services;
using UpTulse.Shared.Services.Impl;

namespace UpTulse.Application
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            services.AddMapperConfigs();
            services.AddServices();
            services.AddModelsValidators();

            return services;
        }

        private static void AddMapperConfigs(this IServiceCollection services)
        {
            services.AddScoped<MonitoringTargetMapperWithDi>();
        }

        private static void AddModelsValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<MonitoringTargetRequest>, MonitoringTargetRequestValidator>();
        }

        private static void AddServices(this IServiceCollection services)
        {
            services.AddHttpClient();

            services.AddScoped<IClaimService, ClaimService>();
            services.AddScoped<IMonitoringTargetService, MonitoringTargetService>();
            services.AddScoped<IMonitoringGroupService, MonitoringGroupService>();

            services.AddSingleton<IMonitoringManagerService, MonitoringManagerService>();
        }
    }
}