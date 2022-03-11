using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace SS.Template.Core.Persistence
{
    public interface IRepositoryBase
    {
        /// <summary>
        /// Returns a query using the specified predicate.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="includes">The include expressions.</param>
        IQueryable<T> Query<T>(Expression<Func<T, bool>> predicate,
            IEnumerable<Expression<Func<T, object>>> includes) where T : class;

        T Get<T, TKey>(TKey id)
            where T : class
            where TKey : IEquatable<TKey>;

        IQueryable<T> Include<T, TProperty>(IQueryable<T> query,
            Expression<Func<T, TProperty>> navigationPropertyPath)
            where T : class;

        #region Async Methods

        Task<T> SingleAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default);

        Task<T> FirstAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default);

        Task<bool> AnyAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default);

        Task ForEachAsync<T>(IQueryable<T> query, Action<T> action, CancellationToken cancellationToken = default);

        Task<bool> AllAsync<T>(IQueryable<T> query, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        Task<int> CountAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default);

        Task<List<T>> ListAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default);

        #endregion Async Methods
    }
}
