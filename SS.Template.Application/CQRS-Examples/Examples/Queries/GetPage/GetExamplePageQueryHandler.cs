using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using SS.Template.Application.Infrastructure;
using SS.Template.Application.Queries;
using SS.Template.Core.Persistence;
using SS.Template.Domain.Entities;
using SS.Template.Domain.Model;

namespace SS.Template.Application.Examples.Queries.GetPage
{
    public sealed class GetExamplePageQueryHandler : IQueryHandler<GetExamplePageQuery, PaginatedResult<ExampleModel>>
    {
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IMapper _mapper;
        private readonly IPaginator _paginator;

        public GetExamplePageQueryHandler(IReadOnlyRepository readOnlyRepository, IMapper mapper, IPaginator paginator)
        {
            _readOnlyRepository = readOnlyRepository;
            _mapper = mapper;
            _paginator = paginator;
        }

        public async Task<PaginatedResult<ExampleModel>> Handle(GetExamplePageQuery request, CancellationToken cancellationToken)
        {
            var query = _readOnlyRepository.Query<Example>(x => x.Status == EnabledStatus.Enabled);

            if (!string.IsNullOrEmpty(request.Term))
            {
                var term = request.Term.Trim();
                query = query.Where(x => x.Name.Contains(term));
            }

            var sortCriteria = request.GetSortCriteria();
            var items = query
                .ProjectTo<ExampleModel>(_mapper.ConfigurationProvider)
                .OrderByOrDefault(sortCriteria, x => x.Name);
            var page = await _paginator.MakePageAsync(_readOnlyRepository, query, items, request, cancellationToken);

            return page;
        }
    }
}
