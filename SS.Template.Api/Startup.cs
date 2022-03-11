using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using SS.Template.Api.Filters;
using SS.Template.Api.Identity;
using SS.Template.Application.Commands;
using SS.Template.Application.Examples;
using SS.Template.Application.Infrastructure;
using SS.Template.Core;
using SS.Template.Core.Persistence;
using SS.Template.Infrastructure;
using SS.Template.Infrastructure.DependencyInjection;
using SS.Template.Persistence;

namespace SS.Template.Api
{
    public class Startup
    {
        private const string AssetsDirectory = "assets";

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder =>
                builder.AllowAnyHeader()
                        .WithExposedHeaders(HeaderNames.ContentDisposition)
                        .AllowAnyMethod()
                        .AllowCredentials()
                        //.WithOrigins(corsConfig["Origins"].Split(','))
                        .WithOrigins(Configuration["ClientDomain"]));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var applicationAssembly = typeof(ExamplesMapping).Assembly;

            services.AddControllers();

            services.AddHealthChecks();

            AddIdentity(services);
            AddAuthentication(services, Configuration);

            AddMvcCore(services, applicationAssembly);
            AddAppServices(services, applicationAssembly);
        }

        private static void AddAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
                    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                })
                .AddCookie(IdentityConstants.ApplicationScheme, options =>
                {
                    options.Cookie.Name = AuthConstants.CookieName;
                    options.Events.OnRedirectToLogin = context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        return Task.CompletedTask;
                    };

                    options.Events.OnRedirectToAccessDenied = context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        return Task.CompletedTask;
                    };
                    options.ForwardDefaultSelector = ctx =>
                    {
                        var authHeader = ctx.Request.Headers[HeaderNames.Authorization];
                        if (authHeader.Count > 0)
                        {
                            return AuthConstants.BearerScheme;
                        }
                        return IdentityConstants.ApplicationScheme;
                    };
                })
                .AddScheme<SecretBearerAuthenticationOptions, SecretBearerAuthenticationHandler>(
                    AuthConstants.BearerScheme, "Bearer",
                    options =>
                    {
                        options.Secret = configuration["ApiSecret"];
                        options.ClaimsIssuer = AuthConstants.BearerScheme;
                    })
                .AddCookie(IdentityConstants.ExternalScheme, o =>
                {
                    o.Cookie.Name = IdentityConstants.ExternalScheme;
                    o.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                })
                .AddCookie(IdentityConstants.TwoFactorRememberMeScheme, o =>
                {
                    o.Cookie.Name = IdentityConstants.TwoFactorRememberMeScheme;
                    o.Events = new CookieAuthenticationEvents
                    {
                        OnValidatePrincipal = SecurityStampValidator.ValidateAsync<ITwoFactorSecurityStampValidator>
                    };
                })
                .AddCookie(IdentityConstants.TwoFactorUserIdScheme, o =>
                {
                    o.Cookie.Name = IdentityConstants.TwoFactorUserIdScheme;
                    o.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                });
        }

        /// <summary>
        /// Automatically registers validators for <see cref="Edit{T,TResponse}" /> commands.
        /// </summary>
        /// <param name="services">The services.</param>
        private static void AddEditCommandValidators(IServiceCollection services)
        {
            var descriptors = new List<ServiceDescriptor>();

            foreach (var descriptor in services.Where(x => IsClosedType(x.ServiceType, typeof(IRequestHandler<,>))))
            {
                var commandType = descriptor.ServiceType.GetGenericArguments()[0];
                if (commandType.BaseType != null && IsClosedType(commandType.BaseType, typeof(EditBase<>)))
                {
                    var modelType = commandType.BaseType.GetGenericArguments()[0];
                    var validatorInterfaceType = typeof(IValidator<>).MakeGenericType(commandType);
                    var validatorConcreteType = typeof(EditValidator<,>).MakeGenericType(commandType, modelType);
                    descriptors.Add(ServiceDescriptor.Transient(validatorInterfaceType, validatorConcreteType));
                }
            }

            foreach (var descriptor in descriptors)
            {
                services.Add(descriptor);
            }
        }

        private static void AddIdentity(IServiceCollection services)
        {
            services
                .AddIdentityCore<User>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequiredLength = AuthConstants.MinPasswordLength;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireDigit = true;
                })
                .AddRoles<Role>()
                .AddSignInManager()
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();
        }

        private static void AddMvcCore(IServiceCollection services, Assembly applicationAssembly)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressMapClientErrors = true;
            });
            services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
            });

            services
                .AddMvcCore(options =>
                {
                    options.Filters.Add(new ExceptionHandlerFilterAttribute());
                    options.Filters.Add(new AuthorizeFilter(AuthConstants.DefaultPolicy));
                })
                .AddAuthorization(options =>
                {
                    options.AddPolicy(AuthConstants.BearerPolicy, builder => builder.AddAuthenticationSchemes(AuthConstants.BearerScheme)
                        .RequireAuthenticatedUser());
                    options.AddPolicy(AuthConstants.DefaultPolicy, builder => builder.RequireAuthenticatedUser());
                    options.DefaultPolicy = options.GetPolicy(AuthConstants.DefaultPolicy);
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddFluentValidation(options =>
                {
                    options.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                    options.RegisterValidatorsFromAssemblies(new[] { applicationAssembly, typeof(Startup).Assembly });
                });
        }

        private static bool IsClosedType(Type type, Type openGenericType)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == openGenericType;
        }

        private void AddAppServices(IServiceCollection services, Assembly applicationAssembly)
        {
            services.AddScoped<IdentityInitializer>();
            var useRowNumberForPaging = Configuration.GetValue<bool>("UseRowNumberForPaging");

            void SqlServerOptionsAction(SqlServerDbContextOptionsBuilder b)
            {
                if (useRowNumberForPaging)
                {
                    b.UseRowNumberForPaging();
                }
            }

            // DbContext's
            services.AddDbContext<AuthDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("AuthConnection"), SqlServerOptionsAction);
            });
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("AppConnection"), SqlServerOptionsAction);
            });
            services.AddScoped<AppDbContextInitializer>();

            // Persistence
            services.AddTransient<IRepository, EfRepository<AppDbContext>>();
            services.AddTransient<IReadOnlyRepository, ReadOnlyEfRepository<AppDbContext>>();
            services.AddSingleton<IPaginator, DefaultPaginator>();

            // Infrastructure
            services.AddSingleton<IDateTime>(new SystemDateTime(Configuration["TimeZone"]));
            services.AddAutoMapper(AutoMapperConfig.Configure, applicationAssembly);

            if (Environment.IsDevelopment())
            {
                var root = Path.Join(Environment.ContentRootPath, AssetsDirectory);
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }
                services.AddSingleton<IFileSystem>(new LocalFileSystem(root));
                services.AddSingleton<IUrlService>(new DomainUrlService(Configuration.GetValue<Uri>("AssetsDomain"), AssetsDirectory,
                    Configuration.GetValue<Uri>("ClientDomain")));
                // If SMTP configuration exists for development, use it
                if (!RegisterSmtpSender(services, false))
                {
                    services.AddSingleton<IEmailSender>(new DummyEmailSender(Path.Join(Environment.ContentRootPath, "Logs")));
                }
            }
            else
            {
                RegisterSmtpSender(services, required: true);
                services.AddSingleton<IUrlService>(new DomainUrlService(Configuration.GetValue<Uri>("AssetsDomain"), null,
                    Configuration.GetValue<Uri>("ClientDomain")));
            }

            // Services
            services.AddServices(applicationAssembly);

            // MediatR
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            services.AddMediatR(applicationAssembly);

            AddEditCommandValidators(services);
        }

        private bool RegisterSmtpSender(IServiceCollection services, bool required)
        {
            var smtpSection = Configuration.GetSection("SMTP");
            var children = smtpSection.GetChildren();
            if (!children.Any())
            {
                if (required)
                {
                    throw new SettingsException("No SMTP configuration found.");
                }

                return false;
            }

            services.ConfigureAndValidate<SmtpSettings>(smtpSection);
            services.AddTransient<IEmailSender, SmtpEmailSender>();
            return true;
        }
    }
}
