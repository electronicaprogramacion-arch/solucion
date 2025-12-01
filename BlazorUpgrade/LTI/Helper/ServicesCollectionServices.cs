using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace CalibrationSaaS.Infraestructure.Blazor.Helper
{
    public static class ServicesCollectionServices
    {
        public static IServiceCollection Replace<TService>(
            this IServiceCollection services,
            Func<IServiceProvider, TService> implementationFactory,
            ServiceLifetime lifetime)
            where TService : class
        {
            var descriptorToRemove = services.FirstOrDefault(d => d.ServiceType == typeof(TService));

            services.Remove(descriptorToRemove);

            var descriptorToAdd = new ServiceDescriptor(typeof(TService), implementationFactory, lifetime);

            services.Add(descriptorToAdd);

            return services;
        }
    }
}