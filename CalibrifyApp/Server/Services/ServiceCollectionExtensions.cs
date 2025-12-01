using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CalibrifyApp.Server.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNullStateFacade(this IServiceCollection services)
        {
            // Register our null implementation
            services.AddScoped<INullStateFacade, NullStateFacade>();

            // Try to remove any existing registration for IStateFacade
            services.RemoveAll(typeof(Blazed.Controls.IStateFacade));

            // Return the service collection for chaining
            return services;
        }
    }
}
