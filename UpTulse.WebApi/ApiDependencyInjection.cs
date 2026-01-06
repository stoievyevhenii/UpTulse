using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using UpTulse.Core.Exceptions;
using UpTulse.DataAccess.EnvironmentVariables;

namespace UpTulse.WebApi
{
    public static class ApiDependencyInjection
    {
        public static IServiceCollection AddJwtAuthViaBearer(this IServiceCollection services, IConfiguration configuration)
        {
            var secretKey = Environment.GetEnvironmentVariable(SecurityEnv.JWT_SECRET);

            var key = Encoding.ASCII.GetBytes(secretKey ?? "Y5srr16jA0G6bp74");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            return services;
        }
    }
}