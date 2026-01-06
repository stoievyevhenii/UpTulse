using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using UpTulse.Core.Exceptions;
using UpTulse.DataAccess.EnvironmentVariables;
using UpTulse.DataAccess.Identity;
using UpTulse.DataAccess.Persistence;
using UpTulse.DataAccess.Repositories;
using UpTulse.DataAccess.Repositories.Impl;

namespace UpTulse.DataAccess
{
    public static class DataAccessDependencyInjection
    {
        public static IServiceCollection AddDataAccessLayer(this IServiceCollection services)
        {
            services.AddDatabase();
            services.AddIdentity();
            services.AddRepositories();

            return services;
        }

        private static void AddDatabase(this IServiceCollection services)
        {
            var connectionString = Environment.GetEnvironmentVariable(SystemEnv.CONNECTION_STRING)
                ?? throw new EnvironmentVariableNotFoundException(SystemEnv.CONNECTION_STRING);

            services.AddDbContext<DatabaseContext>(options =>
                            options.UseNpgsql(connectionString,
                                opt =>
                                {
                                    opt.MigrationsAssembly(typeof(DatabaseContext).Assembly.FullName);
                                    opt.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                                }));
        }

        private static void AddIdentity(this IServiceCollection services)
        {
            services.AddDefaultIdentity<ApplicationUser>(
                options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<DatabaseContext>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 15;
                options.Lockout.AllowedForNewUsers = true;

                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ISettingRepository, SettingRepository>();
            services.AddScoped<IMonitoringGroupRepository, MonitoringGroupRepository>();
            services.AddScoped<IMonitoringTargetRepository, MonitoringTargetRepository>();
            services.AddScoped<IMonitoringHistoryRepository, MonitoringHistoryRepository>();
        }
    }
}