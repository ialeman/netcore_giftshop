using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SS.Template.Core.Persistence;

namespace SS.Template.Persistence
{
    public class EfRepository : EfRepositoryBase, IRepository
    {
        public EfRepository(DbContext context)
            : base(context, false)
        {
        }

        public IReadOnlyRepository AsReadOnly()
        {
            return new ReadOnlyEfRepository(Context);
        }

        public virtual T Add<T>(T entity)
            where T : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var result = Set<T>().Add(entity);

            return result.Entity;
        }

        public virtual T Remove<T>(T entity)
            where T : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var result = Set<T>().Remove(entity);

            return result.Entity;
        }

        public virtual T Update<T>(T entity)
            where T : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var result = Set<T>().Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;

            return result.Entity;
        }

        public virtual Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return Context.SaveChangesAsync(cancellationToken);
        }
    }

    public class EfRepository<TContext> : EfRepository
        where TContext : DbContext
    {
        public EfRepository(TContext context)
            : base(context)
        {
        }
    }
}
