using System;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SS.Template.Api;
using SS.Template.Data;
using SS.Template.Persistence;
using SS.Template.Services;
using SS.Template.DependencyInjection;
using SS.Template.Api.Identity;

namespace SS.Template.Api.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {

        public static void AddAppServices(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            services.AddMemoryCache();
            services.AddSingleton<IMapper>(new Mapper(new MapperConfiguration(AutoMapperConfig.Configure)));

            services.AddScoped<IDbContextFactory<AppDbContext>, AppDbContextFactory>();

            //Register repositories
            services.AddTransient<AppDbContextInitializer>();
            //services.AddScoped   <AppDbContextInitializer>();

            services.AddTransient<IAppRepository, AppRepository>();
            services.AddTransient<IAppReadOnlyRepository, AppReadOnlyRepository>();
            services.AddScoped<IAppDatabaseFactory, AppDatabaseFactory>();
            
            // Register services
            services.AddSingleton(configuration);
            services.AddServices(ServiceLifetime.Scoped, typeof(Startup).Assembly, typeof(AutoMapperConfig).Assembly);

            services.AddSingleton<IHttpClientFactory, HttpClientFactory>();

            // services.AddTransient<ITemplateEngine, RazorTemplateEngine>();
            //services.AddTransient<ITemplateEmailSender, TemplateEmailSender>();

            // services.AddScoped<ICurrentUserService, HttpCurrentUserService>();
            //services.AddSettings(typeof(UsersServiceSettings).Assembly);

            services.AddLazy();

            // Identity
            services.AddTransient<IdentityInitializer>();

        }

        //public static IServiceCollection AddServices(this IServiceCollection services, params Assembly[] assemblies)
        //{
        //    return services.AddServices(ServiceLifetime.Transient, assemblies);
        //}

        //public static IServiceCollection AddServices(this IServiceCollection services, ServiceLifetime lifetime, params Assembly[] assemblies)
        //{
        //    if (services == null)
        //    {
        //        throw new ArgumentNullException(nameof(services));
        //    }

        //    if (assemblies == null)
        //    {
        //        throw new ArgumentNullException(nameof(assemblies));
        //    }

        //    var serviceTypes = assemblies.SelectMany(x => x.GetTypes())
        //        .Where(x => x.IsClass && !x.IsAbstract && x.Name.EndsWith("Service"));

        //    foreach (var serviceType in serviceTypes)
        //    {
        //        AddService(services, serviceType, lifetime);
        //    }

        //    return services;
        //}

        //public static IServiceCollection AddService(this IServiceCollection services, Type type, ServiceLifetime lifetime = ServiceLifetime.Transient)
        //{
        //    if (services == null)
        //    {
        //        throw new ArgumentNullException(nameof(services));
        //    }

        //    if (type == null)
        //    {
        //        throw new ArgumentNullException(nameof(type));
        //    }

        //    var interfaces = type.GetInterfaces();
        //    if (interfaces.Length > 0)
        //    {
        //        foreach (var interfaceType in interfaces)
        //        {
        //            services.Add(new ServiceDescriptor(interfaceType, type, lifetime));
        //        }
        //    }
        //    else
        //    {
        //        services.Add(new ServiceDescriptor(type, type, lifetime));
        //    }

        //    return services;
        //}
        
    }
}
