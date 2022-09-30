using System;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Microsoft.Extensions.Hosting;
using SS.Template.Persistence;

namespace SS.Template.Services
{
    public sealed class AppDbContextFactory : IDbContextFactory<AppDbContext>, IDisposable
    {
        private static readonly HashSet<Guid> InitializedDatabases = new HashSet<Guid>();
        private readonly SqlServerOptionsExtension _extension;
        private readonly Lazy<AppDbContext> _lazyInstance;
        private readonly DbContextOptions<AppDbContext> _options;
        private readonly IHostEnvironment _environment;
        private bool _disposed;

        public AppDbContextFactory(DbContextOptions<AppDbContext> options, IHostEnvironment environment)
        {
            _options = options;
            _environment = environment;

            _extension = _options.FindExtension<SqlServerOptionsExtension>();

            if (_extension == null)
            {
                // This must be an in-memory DbContext
                _lazyInstance = new Lazy<AppDbContext>(() => new AppDbContext(_options));
            }
            else
            {
                _lazyInstance = new Lazy<AppDbContext>(CreateInternal);
            }
        }

        public AppDbContext Create()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(AppDbContextFactory));
            }

            return _lazyInstance.Value;
        }

        public DbConnection CreateConnection()
        {
            //var id = _currentSponsorService.Get();
            //if (id == null)
            //{
            //    throw new InvalidOperationException("Could not resolve the current sponsor Id.");
            //}
            var connectionString = GetConnectionString();
            return new SqlConnection(connectionString);
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            if (_lazyInstance.IsValueCreated)
            {
                _lazyInstance.Value.Dispose();
            }
            _disposed = true;
        }

        private static void Initialize(AppDbContext context, IHostEnvironment environemt)
        {
            //if (!InitializedDatabases.Contains(id))
            //{
            //    lock (InitializedDatabases)
            //    {
            //        if (!InitializedDatabases.Contains(id))
            //        {
            var initializer = new AppDbContextInitializer(context, environemt);
            AsyncHelpers.RunSync(() => initializer.Run());
            // InitializedDatabases.Add();
            //        }
            //    }
            //}
        }

        private AppDbContext CreateInternal()
        {
            //var id = _currentSponsorService.Get();
            //if (id == null)
            //{
            //    throw new InvalidOperationException();
            //}

            var connectionString = GetConnectionString();
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>(_options);
            optionsBuilder.UseSqlServer(connectionString);

            var options = optionsBuilder.Options;
            var context = new AppDbContext(options);

            Initialize(context, _environment);

            return context;
        }

        private string GetConnectionString()
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder(_extension.ConnectionString);
            connectionStringBuilder.InitialCatalog = $"{connectionStringBuilder.InitialCatalog}";
            return connectionStringBuilder.ConnectionString;
        }
    }
}
