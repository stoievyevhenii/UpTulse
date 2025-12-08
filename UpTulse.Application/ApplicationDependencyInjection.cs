using Microsoft.Extensions.DependencyInjection;

using UpTulse.Application.Services;
using UpTulse.Application.Services.Impl;

namespace UpTulse.Application
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            services.AddServices();

            return services;
        }

        private static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IMonitoringTargetService, MonitoringTargetService>();
        }
    }
}