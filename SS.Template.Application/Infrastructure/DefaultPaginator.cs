using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SS.Template.Application.Queries;
using SS.Template.Core.Persistence;

namespace SS.Template.Application.Infrastructure
{
    public class DefaultPaginator : IPaginator
    {
        public Task<PaginatedResult<TItem>> MakePageAsync<TCount, TItem>(
            IRepositoryBase repository,
            IQueryable<TCount> countQuery,
            IQueryable<TItem> itemsQuery,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default)
            where TCount : class where TItem : class
        {
            ValidateMakePageParams(repository, countQuery, itemsQuery);

            return MakePageInternalAsync(repository, countQuery, itemsQuery, page, pageSize, cancellationToken);
        }

        private static void ValidateMakePageParams<TCount, TItem>(IRepositoryBase repository,
            IQueryable<TCount> countQuery,
            IQueryable<TItem> itemsQuery)
            where TCount : class
            where TItem : class
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            if (countQuery == null)
            {
                throw new ArgumentNullException(nameof(countQuery));
            }

            if (itemsQuery == null)
            {
                throw new ArgumentNullException(nameof(itemsQuery));
            }
        }

        private static async Task<PaginatedResult<TItem>> MakePageInternalAsync<TCount, TItem>(
            IRepositoryBase repository,
            IQueryable<TCount> countQuery,
            IQueryable<TItem> itemsQuery,
            int page,
            int pageSize,
            CancellationToken cancellationToken)
            where TCount : class
            where TItem : class
        {
            Paginator.ValidatePaging(page, pageSize);

            var count = await repository.CountAsync(countQuery, cancellationToken);

            var items = await repository.ListAsync(itemsQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize), cancellationToken);

            return PaginatedResult.From(items, count, page, pageSize);
        }
    }
}
