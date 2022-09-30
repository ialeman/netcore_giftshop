using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SS.Template.DependencyInjection
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

        public static IServiceCollection AddSettings(this IServiceCollection services,
            params Assembly[] assemblies)
        {
            return services.AddSettings(ServiceLifetime.Singleton, assemblies);
        }

        public static IServiceCollection AddSettings(this IServiceCollection services, ServiceLifetime lifetime, params Assembly[] assemblies)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (assemblies == null)
            {
                throw new ArgumentNullException(nameof(assemblies));
            }

            var settingsTypes = assemblies.SelectMany(x => x.GetTypes())
                .Where(x => x.IsClass && !x.IsAbstract && typeof(ISettingsObject).IsAssignableFrom(x));

            foreach (var type in settingsTypes)
            {
                services.AddSettings(type, lifetime);
            }

            return services;
        }

        public static IServiceCollection AddSettings(this IServiceCollection services,
            Type type, ServiceLifetime lifetime = ServiceLifetime.Singleton)
        {
            var constructor = type
                .GetConstructors()
                .SingleOrDefault(x => x.GetParameters().Any());

            if (constructor == null)
            {
                throw new InvalidOperationException($"Type {type.FullName} has no constructor with injectable parameters.");
            }

            //var instance = CreateSettingsInstance(configuration, constructor);

            services.Add(new ServiceDescriptor(type, provider => CreateSettingsInstance(provider, constructor), lifetime));
            //return services.AddSingleton(type, instance);
            return services;
        }

        public static IServiceCollection AddLazy(this IServiceCollection services)
        {
            return services.AddTransient(typeof(Lazy<>), typeof(LazyService<>));
        }

        private static object CreateSettingsInstance(IServiceProvider serviceProvider, ConstructorInfo constructor)
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            var section = constructor.DeclaringType.GetCustomAttribute<ConfigurationSectionAttribute>();
            if (section != null)
            {
                configuration = configuration.GetSection(section.Name);
            }

            var parameters = constructor.GetParameters();

            var values = new List<object>(parameters.Length);
            foreach (var parameterInfo in parameters)
            {
                var attribute = parameterInfo.GetCustomAttribute<ConfigurationValueAttribute>();
                var settingName = attribute?.Key ?? parameterInfo.Name;
                var value = configuration.GetValue(parameterInfo.ParameterType, settingName, parameterInfo.HasDefaultValue ? parameterInfo.DefaultValue : null);

                values.Add(value);
            }

            var instance = constructor.Invoke(values.ToArray());
            return instance;
        }

        private class LazyService<T> : Lazy<T>, IDisposable
        {
            private bool _disposed;

            public LazyService(IServiceProvider provider)
                : base(provider.GetRequiredService<T>)
            {
            }

            public void Dispose()
            {
                if (!IsValueCreated)
                {
                    return;
                }

                if (_disposed)
                {
                    return;
                }

                if (Value is IDisposable disposable)
                {
                    disposable.Dispose();
                }

                _disposed = true;
            }
        }
    }
}
