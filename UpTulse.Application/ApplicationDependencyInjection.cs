using Microsoft.Extensions.DependencyInjection;

using UpTulse.Application.MapperConfigs;
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

            return services;
        }

        private static void AddMapperConfigs(this IServiceCollection services)
        {
            services.AddScoped<MonitoringTargetMapperWithDi>();
        }

        private static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IClaimService, ClaimService>();
            services.AddScoped<IMonitoringTargetService, MonitoringTargetService>();
            services.AddScoped<IMonitoringGroupService, MonitoringGroupService>();
        }
    }
}