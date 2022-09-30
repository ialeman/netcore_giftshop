using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SS.Data;

namespace SS.Template.Persistence
{
    public abstract class EfRepositoryBase : IRepositoryBase
    {
        private readonly bool _isReadOnly;
        protected DbContext Context { get; }

        protected EfRepositoryBase(DbContext context, bool isReadOnly)
        {
            _isReadOnly = isReadOnly;
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        protected virtual DbSet<T> Set<T>()
            where T : class
        {
            return Context.Set<T>();
        }

        public virtual IQueryable<T> Query<T>(Expression<Func<T, bool>> condition, IEnumerable<Expression<Func<T, object>>> includes)
            where T : class
        {
            var query = (IQueryable<T>)Set<T>();

            if (_isReadOnly)
            {
                query = query.AsNoTracking();
            }

            if (condition != null)
            {
                query = query.Where(condition);
            }

            return WithIncludes(query, includes);
        }

        public virtual T Get<T, TKey>(TKey id)
            where T : class
            where TKey : IEquatable<TKey>
        {
            return Set<T>().Find(id);
        }

        public IQueryable<T> Include<T, TProperty>(IQueryable<T> query,
            Expression<Func<T, TProperty>> navigationPropertyPath)
            where T : class
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            if (navigationPropertyPath == null)
            {
                throw new ArgumentNullException(nameof(navigationPropertyPath));
            }

            return query.Include(navigationPropertyPath);
        }

        #region Async methods

        public async Task<T> FirstAsync<T>(IQueryable<T> query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> AnyAsync<T>(IQueryable<T> query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            return await query.AnyAsync();
        }

        public async Task<bool> AllAsync<T>(IQueryable<T> query, Expression<Func<T, bool>> predicate)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return await query.AllAsync(predicate);
        }

        public async Task ForEachAsync<T>(IQueryable<T> query, Action<T> action)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            await query.ForEachAsync(action);
        }

        public async Task<int> CountAsync<T>(IQueryable<T> query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            return await query.CountAsync();
        }

        [Obsolete("Wrongly named. Use ListAsync instead.")]
        public async Task<List<T>> ToListAsync<T>(IQueryable<T> query, IEnumerable<Expression<Func<T, object>>> includes)
            where T : class
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            return await WithIncludes(query, includes).ToListAsync();
        }

        public async Task<List<T>> ListAsync<T>(IQueryable<T> query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            return await query.ToListAsync();
        }

        private static IQueryable<T> WithIncludes<T>(IQueryable<T> query, IEnumerable<Expression<Func<T, object>>> includes)
            where T : class
        {
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            return query;
        }

        #endregion Async methods

        /*
        public IEnumerable<TEntity> SqlQuery<TEntity>(string sql, params object[] parameters)
        {
            throw new NotImplementedException();
        }
        */

    }
}
