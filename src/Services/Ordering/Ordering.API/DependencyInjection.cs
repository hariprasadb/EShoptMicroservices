using BuildingBlocks.Exceptions.Handler;
using Carter;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Ordering.API
{
    internal static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services,
                                                       IConfiguration configuration)
        {
            services.AddCarter();
            services.AddExceptionHandler<CustomExceptionHandler>();

            /*
            //https://www.nuget.org/packages/AspNetCore.HealthChecks.SqlServer
            //https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks
            //AspNetCore.HealthChecks.UI.Client
            //https://www.nuget.org/packages/AspNetCore.HealthChecks.UI.Client
            //https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-10.0
            */
            services.AddHealthChecks()
                .AddSqlServer(configuration.GetConnectionString("Database"));

     
            return services;
        }
        public static WebApplication UseApiServices(this WebApplication app)
        {
            app.MapCarter();
            app.UseExceptionHandler((options) => { });
            app.UseHealthChecks("/health", new HealthCheckOptions()
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            return app;
        }
    }
}
