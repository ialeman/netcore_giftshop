using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace SS.Template.Persistence
{
    public interface IDbContextFactory<out TContext>
        where TContext : DbContext
    {
        TContext Create();

        DbConnection CreateConnection();
    }
}
