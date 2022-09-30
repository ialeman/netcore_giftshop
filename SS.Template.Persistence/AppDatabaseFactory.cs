using AsyncPoco;
using SS.Template.Persistence;

namespace SS.Template.Data
{
    public interface IAppDatabaseFactory
    {
        IDatabase Create();
    }

    public sealed class AppDatabaseFactory : IAppDatabaseFactory
    {
        private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

        public AppDatabaseFactory(IDbContextFactory<AppDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public IDatabase Create()
        {
            return Database.Create(() => _dbContextFactory.CreateConnection());
        }
    }
}
