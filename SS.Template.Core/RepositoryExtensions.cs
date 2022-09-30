using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SS.Data
{
    public static class RepositoryExtensions
    {
        /// <summary>
        /// Returns a query without a predicate
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="repository">The <see cref="IRepository"/> instance.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="repository" /> is <c>null</c>
        /// </exception>
        public static IQueryable<T> Query<T>(this IRepositoryBase repository)
            where T : class
        {
            return repository.Query(default(Expression<Func<T, bool>>), default(IEnumerable<Expression<Func<T, object>>>));
        }

        /// <summary>
        /// Returns a query without a predicate and a number of include expressions.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="repository">The <see cref="IRepository"/> instance.</param>
        /// <param name="includes">The include expressions.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="repository" /> is <c>null</c>
        /// </exception>
        public static IQueryable<T> Query<T>(this IRepositoryBase repository, params Expression<Func<T, object>>[] includes)
            where T : class
        {
            return repository.Query(default(Expression<Func<T, bool>>), includes);
        }

        /// <summary>
        /// Returns a query without a predicate and a number of include expressions.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="repository">The <see cref="IRepository"/> instance.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="includes">The include expressions.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="repository" /> is <c>null</c>
        /// </exception>
        public static IQueryable<T> Query<T>(this IRepositoryBase repository, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
            where T : class
        {
            return repository.Query(predicate, includes);
        }

        /// <summary>
        /// Adds the specified entities.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="repository">The <see cref="IRepository"/> instance.</param>
        /// <param name="entities">The entities to be added.</param>
        public static IEnumerable<T> Add<T>(this IRepository repository, IEnumerable<T> entities) where T : class
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            return entities.Select(repository.Add);
        }

        /// <summary>
        /// Returns the count of the given entity using the specified condition.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="repository">The <see cref="IRepository"/> instance.</param>
        /// <param name="predicate">The predicate.</param>
        public static int Count<T>(this IRepositoryBase repository, Expression<Func<T, bool>> predicate) where T : class
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            return repository.Query(predicate).Count();
        }

        /// <summary>
        /// Returns the count of the given entity.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="repository">The <see cref="IRepository"/> instance.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="repository" /> is <c>null</c>
        /// </exception>
        public static int Count<T>(this IRepositoryBase repository) where T : class
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            return repository.Count(default(Expression<Func<T, bool>>));
        }

        /// <summary>
        /// Removes the entity specified in the expression.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="repository">The <see cref="IRepository"/> instance.</param>
        /// <param name="predicate">The expression.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="repository" /> or <paramref name="predicate" /> is <c>null</c>
        /// </exception>
        public static void Remove<T>(this IRepository repository, Expression<Func<T, bool>> predicate) where T : class
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            foreach (var entity in repository.Query<T>().Where(predicate))
            {
                repository.Remove(entity);
            }
        }

        /// <summary>
        /// Removes the specified entities.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="repository">The <see cref="IRepository"/> instance.</param>
        /// <param name="entities">The entities to be deleted.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="repository" /> or <paramref name="entities" /> is <c>null</c>
        /// </exception>
        public static void RemoveAll<T>(this IRepository repository, IEnumerable<T> entities) where T : class
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            foreach (var entity in entities)
            {
                repository.Remove(entity);
            }
        }

        /// <summary>
        /// Gets the first entity in the set ordering by the specified property.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="repository">The <see cref="IRepository"/> instance.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="repository" /> is <c>null</c>
        /// </exception>
        public static T First<T>(this IRepositoryBase repository) where T : class
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            return repository.Query<T>().FirstOrDefault();
        }

        /// <summary>
        /// Gets the first entity in the set ordering by the specified property.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <typeparam name="TProperty">The order property type.</typeparam>
        /// <param name="repository">The <see cref="IRepository"/> instance.</param>
        /// <param name="orderSelector">A function to extract a key from an element.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="repository" /> or <paramref name="orderSelector" /> is <c>null</c>
        /// </exception>
        public static T First<T, TProperty>(this IRepositoryBase repository, Expression<Func<T, TProperty>> orderSelector) where T : class
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            if (orderSelector == null)
            {
                throw new ArgumentNullException(nameof(orderSelector));
            }

            return repository.Query<T>().OrderBy(orderSelector).FirstOrDefault();
        }
    }
}
