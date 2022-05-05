using ToDoList.Business.Enums;
using ToDoList.Business.Models;

namespace ToDoList.Business.Repositories;

public interface ICategoryRepository : IRepository
{
    Task<CategoryModel> GetByIdAsync(int id);
    Task<List<CategoryModel>> GetAsync(string? like, CategoriesSortOrder sortOrder);
    Task<CategoryModel> GetByIdWithTodosAsync(int id);
    Task<CategoryModel> CreateAsync(CategoryModel category);
    Task<CategoryModel> UpdateAsync(CategoryModel category);
    Task RemoveAsync(int id);

}
