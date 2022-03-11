using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace SS.Template.Infrastructure.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, params Assembly[] assemblies)
        {
            return services.AddServices(ServiceLifetime.Transient, assemblies);
        }

        public static IServiceCollection AddServices(this IServiceCollection services, ServiceLifetime lifetime, params Assembly[] assemblies)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (assemblies == null)
            {
                throw new ArgumentNullException(nameof(assemblies));
            }

            var serviceTypes = assemblies.SelectMany(x => x.GetTypes())
                .Where(x => x.IsClass && !x.IsAbstract && x.Name.EndsWith("Service"));

            foreach (var serviceType in serviceTypes)
            {
                AddService(services, serviceType, lifetime);
            }

            return services;
        }

        public static IServiceCollection AddService(this IServiceCollection services, Type type, ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var interfaces = type.GetInterfaces();
            if (interfaces.Length > 0)
            {
                foreach (var interfaceType in interfaces)
                {
                    services.Add(new ServiceDescriptor(interfaceType, type, lifetime));
                }
            }
            else
            {
                services.Add(new ServiceDescriptor(type, type, lifetime));
            }

            return services;
        }
    }
}
