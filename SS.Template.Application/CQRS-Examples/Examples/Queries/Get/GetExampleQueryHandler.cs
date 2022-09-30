using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using SS.Template.Application.Queries;
using SS.Template.Core.Exceptions;
using SS.Data;
using SS.Template.Domain.Entities;
using SS.Template.Domain.Model;

namespace SS.Template.Application.Examples.Queries.Get
{
    public sealed class GetExampleQueryHandler //: IQueryHandler<GetExampleQuery, ExampleModel>
    {
        //private readonly IReadOnlyRepository _readOnlyRepository;
        //private readonly IMapper _mapper;

        //public GetExampleQueryHandler(IReadOnlyRepository readOnlyRepository, IMapper mapper)
        //{
        //    _readOnlyRepository = readOnlyRepository;
        //    _mapper = mapper;
        //}

        //public async Task<ExampleModel> Handle(GetExampleQuery request, CancellationToken cancellationToken)
        //{
        //    var query = _readOnlyRepository.Query<Example>(x => x.Id == request.Id && x.Status == EnabledStatus.Enabled)
        //        .ProjectTo<ExampleModel>(_mapper.ConfigurationProvider);

        //    var result = await _readOnlyRepository.SingleAsync(query, cancellationToken);

        //    if (result == null)
        //    {
        //        throw EntityNotFoundException.For<Example>(request.Id);
        //    }

        //    return result;
        //}
    }
}
