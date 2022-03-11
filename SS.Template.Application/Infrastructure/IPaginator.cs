using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SS.Template.Application.Queries;
using SS.Template.Core.Persistence;

namespace SS.Template.Application.Infrastructure
{
    public interface IPaginator
    {
        Task<PaginatedResult<TItem>> MakePageAsync<TCount, TItem>(IRepositoryBase repository, IQueryable<TCount> countQuery,
            IQueryable<TItem> itemsQuery, int page, int pageSize, CancellationToken cancellationToken = default)
            where TCount : class where TItem : class;
    }
}
