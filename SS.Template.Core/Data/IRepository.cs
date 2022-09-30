using System;
using System.Threading;
using System.Threading.Tasks;

namespace SS.Data
{
    public interface IRepository : IRepositoryBase
    {
        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="entity">The entity.</param>
        T Add<T>(T entity) where T : class;

        IReadOnlyRepository AsReadOnly();

        /// <summary>
        /// Removes the specified entity.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="entity">The entity.</param>
        T Remove<T>(T entity) where T : class;

        /// <summary>
        /// Commits the changes to the data store.
        /// </summary>
        /// <returns>The number of objects written to the underlying database.</returns>
        int SaveChanges();

        /// <summary>
        /// Asynchronously saves all changes made in this context to the underlying database.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task
        /// to complete.</param>
        /// <returns>A task that represents the asynchronous save operation.  The task result
        /// contains the number of state entries written to the underlying database.
        /// This can include state entries for entities and/or relationships. Relationship
        /// state entries are created for many-to-many relationships and relationships
        /// where there is no foreign key property included in the entity class (often
        /// referred to as independent associations).</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="entity">The entity.</param>
        T Update<T>(T entity) where T : class;
    }
}
