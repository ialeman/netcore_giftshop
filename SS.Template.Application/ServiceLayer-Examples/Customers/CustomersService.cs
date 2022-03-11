using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using SS.Template.Application.Infrastructure;
using SS.Template.Application.Queries;
using SS.Template.Core.Exceptions;
using SS.Template.Core.Persistence;
using SS.Template.Domain.Entities;
using SS.Template.Domain.Model;

namespace SS.Template.Application.Customers
{
    public interface ICustomersService
    {
        Task<PaginatedResult<CustomerModel>> GetPage(PaginatedQuery request);

        Task<CustomerModel> Get(Guid id);

        Task Create(CustomerModel customer);

        Task Update(Guid id, CustomerModel customer);

        Task Delete(Guid id);
    }

    public class CustomersService : ICustomersService
    {
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IMapper _mapper;
        private readonly IPaginator _paginator;
        private readonly IRepository _repository;

        public CustomersService(IReadOnlyRepository readOnlyRepository, IMapper mapper, IPaginator paginator, IRepository repository)
        {
            _readOnlyRepository = readOnlyRepository;
            _mapper = mapper;
            _paginator = paginator;
            _repository = repository;
        }

        public async Task<CustomerModel> Get(Guid id)
        {
            var query = _readOnlyRepository.Query<Customer>(x => x.Id == id && x.Status == EnabledStatus.Enabled)
                .ProjectTo<CustomerModel>(_mapper.ConfigurationProvider);

            var result = await _readOnlyRepository.SingleAsync(query);

            if (result == null)
            {
                throw EntityNotFoundException.For<Customer>(id);
            }

            return result;
        }

        public async Task<PaginatedResult<CustomerModel>> GetPage(PaginatedQuery request)
        {
            var query = _readOnlyRepository.Query<Customer>(x => x.Status == EnabledStatus.Enabled);

            if (!string.IsNullOrEmpty(request.Term))
            {
                var term = request.Term.Trim();
                query = query.Where(x => x.Name.Contains(term));
            }

            var sortCriteria = request.GetSortCriteria();
            var items = query
                .ProjectTo<CustomerModel>(_mapper.ConfigurationProvider)
                .OrderByOrDefault(sortCriteria, x => x.Name);

            var page = await _paginator.MakePageAsync(_readOnlyRepository, query, items, request);
            return page;
        }

        public async Task Create(CustomerModel customer)
        {
            var entity = _mapper.Map<Customer>(customer);

            _repository.Add(entity);

            await _repository.SaveChangesAsync();
        }

        public async Task Update(Guid id, CustomerModel customer)
        {
            var entity = await _repository.FirstAsync<Customer>(x => x.Id == id);

            if (entity == null)
            {
                throw EntityNotFoundException.For<Customer>(id);
            }

            _mapper.Map(customer, entity);
            await _repository.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var entity = await _repository.FirstAsync<Customer>(x => x.Id == id);

            if (entity == null)
            {
                throw EntityNotFoundException.For<Customer>(id);
            }

            _repository.Remove(entity);
            await _repository.SaveChangesAsync();
        }
    }
}
