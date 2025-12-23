using FluentValidation;

using Microsoft.Extensions.DependencyInjection;

using UpTulse.Application.Managers;
using UpTulse.Application.Managers.Impl;
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
            services.AddSystemServices();
            services.AddServices();
            services.AddManagers();
            services.AddModelsValidators();

            return services;
        }

        public static void AddManagers(this IServiceCollection services)
        {
            services.AddSingleton<IMonitoringTargetsManager, MonitoringTargetsManager>();
            services.AddSingleton<INotificationSseManager, NotificationSseManager>();
        }

        private static void AddMapperConfigs(this IServiceCollection services)
        {
            services.AddScoped<MonitoringTargetMapperWithDi>();
        }

        private static void AddModelsValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<MonitoringTargetRequest>, MonitoringTargetRequestValidator>();
            services.AddScoped<IValidator<CreateUserRequest>, CreateUserRequestValidator>();
        }

        private static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IClaimService, ClaimService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMonitoringTargetService, MonitoringTargetService>();
            services.AddScoped<IMonitoringGroupService, MonitoringGroupService>();
            services.AddScoped<IMonitoringHistoryService, MonitoringHistoryService>();
            services.AddScoped<INotificationChannelProviderResolver, NotificationChannelProviderResolver>();
            services.AddScoped<IMonitoringProtocolResolver, MonitoringProtocolResolver>();
        }

        private static void AddSystemServices(this IServiceCollection services)
        {
            services.AddHttpClient();
        }
    }
}