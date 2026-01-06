using Scalar.AspNetCore;

namespace UpTulse.WebApi.Extensions
{
    public static class WebApplicationExtension
    {
        public static WebApplication ConfigureBaseMiddlewares(this WebApplication app)
        {
            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            return app;
        }

        public static WebApplication ConfigureDebugInstruments(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference(options =>
                {
                    options.Authentication = new ScalarAuthenticationOptions
                    {
                        PreferredSecuritySchemes = ["Bearer"]
                    };
                });
            }

            return app;
        }

        public static WebApplication ConfigureSecurity(this WebApplication app)
        {
            app.UseCors(corsPolicyBuilder => corsPolicyBuilder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(_ => true)
                .AllowCredentials()
                );

            return app;
        }
    }
}