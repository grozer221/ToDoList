using ToDoList.Business.Enums;
using ToDoList.Business.Models;
using ToDoList.Business.Repositories;

namespace ToDoList.XML.Repositories
{
    public class XmlCategoryRepository : ICategoryRepository
    {
        public Task<CategoryModel> CreateAsync(CategoryModel category)
        {
            throw new NotImplementedException();
        }

        public Task<List<CategoryModel>> GetAsync(string? like, CategoriesSortOrder sortOrder)
        {
            return Task.FromResult(Get(like, sortOrder));
        }

        public List<CategoryModel> Get(string? like, CategoriesSortOrder sortOrder)
        {
            return new List<CategoryModel>();
        }

        public Task<CategoryModel> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<CategoryModel> GetByIdWithTodosAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<CategoryModel> UpdateAsync(CategoryModel category)
        {
            throw new NotImplementedException();
        }
    }
}
