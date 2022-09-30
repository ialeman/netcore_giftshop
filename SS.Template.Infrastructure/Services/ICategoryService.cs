using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SS.Data;
using SS.Template.Domain.Entities;
using SS.Template.Persistence;

namespace SS.Template.Services
{
    public interface ICategoryService
    {

        Task<bool> AddCategoryAsync(Category category);
        Task<IEnumerable<Category>> GetAll();
        Task<bool> EditCategoryAsync(Category category);
    }

    public class CategoryService : ICategoryService
    {
        private readonly IAppRepository _appRepository;

        public CategoryService(IAppRepository appRepository)
        {
            _appRepository = appRepository;
        }

        public async Task<bool> AddCategoryAsync(Category category)
        {
            _appRepository.Add<Category>(category);
            await _appRepository.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            var repository = _appRepository.AsReadOnly();
            var query = repository.Query<Category>();
            var results = await repository.ListAsync(query);
            return results;
        }

        public async Task<bool> EditCategoryAsync(Category category)
        {
            _appRepository.Update<Category>(category);
            await _appRepository.SaveChangesAsync();
            return true;
        }
    }
}
