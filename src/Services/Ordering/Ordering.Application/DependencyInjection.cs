using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Application
{
    internal static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services ) {
            //services.AddMediatR((cfg =>
            //{
            //    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            //}));
            return services;
        }
    }
}
