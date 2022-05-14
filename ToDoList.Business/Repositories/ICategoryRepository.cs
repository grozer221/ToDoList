using ToDoList.Business.Enums;
using ToDoList.Business.Models;

namespace ToDoList.Business.Repositories;

public interface ICategoryRepository
{
    Task<CategoryModel> GetByIdAsync(int id);
    Task<CategoryModel> GetByIdOrDefaultAsync(int id);
    Task<IEnumerable<CategoryModel>> GetAsync(string? like, CategoriesSortOrder sortOrder);
    Task<IEnumerable<CategoryModel>> GetOrDefaultAsync(string? like, CategoriesSortOrder sortOrder);
    Task<CategoryModel> GetByIdWithTodosAsync(int id);
    Task<CategoryModel> GetByIdWithTodosOrDefaultAsync(int id);
    Task<CategoryModel> CreateAsync(CategoryModel category);
    Task<CategoryModel> UpdateAsync(CategoryModel category);
    Task<CategoryModel> RemoveAsync(int id);
}
