using Microsoft.EntityFrameworkCore;
using SS.Data;

namespace SS.Template.Persistence
{
    public class ReadOnlyEfRepository : EfRepositoryBase, IReadOnlyRepository
    {
        public ReadOnlyEfRepository(DbContext context)
            : base(context, true)
        {
        }
    }

    public class ReadOnlyEfRepository<TContext> : ReadOnlyEfRepository
        where TContext : DbContext
    {
        public ReadOnlyEfRepository(TContext context)
            : base(context)
        {
        }
    }
}
