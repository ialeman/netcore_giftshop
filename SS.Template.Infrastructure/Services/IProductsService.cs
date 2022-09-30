using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SS.Data;
using SS.Template.Domain.Entities;
using SS.Template.Persistence;

namespace SS.Template.Services.Services
{

    public interface IProductsService
    {

        Task<bool> SaveProduct(Product model);

        Task<IEnumerable<Product>> GetAllProducts();

    }

    public class ProductsService : IProductsService
    {
        private readonly IAppRepository _appRepository;

        public ProductsService(IAppRepository appRepository)
        {
            _appRepository = appRepository;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            var repository = _appRepository.AsReadOnly();
            var query =  repository.Query<Product>(x=> x.Category);
            var list = await repository.ListAsync(query);
            return list;
        }

        public async Task<bool> SaveProduct(Product model)
        {
            _appRepository.Add<Product>(model);
            await _appRepository.SaveChangesAsync();
            return true;
        }
    }
}
