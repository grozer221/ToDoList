using ToDoList.Business.Abstractions;
using ToDoList.Business.Enums;
using ToDoList.Business.Models;

namespace ToDoList.Business.Repositories;

public interface ICategoryRepository
{
    int Take { get; }
    Task<CategoryModel> GetByIdAsync(int id);
    Task<GetEntitiesResponse<CategoryModel>> GetAsync(string? like, CategoriesSortOrder sortOrder, int page);
    Task<CategoryModel> CreateAsync(CategoryModel category);
    Task<CategoryModel> UpdateAsync(CategoryModel category);
    Task<CategoryModel> RemoveAsync(int id);
}
