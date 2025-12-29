namespace Ordering.API
{
    internal static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            //service.AddCarter();
            return services;
        }
        public static WebApplication UseAPiServices(this WebApplication app)
        {
            //app.MapCarter();
            return app;
        }
    }
}
