using ToDoList.Business.Enums;
using ToDoList.Business.Models;

namespace ToDoList.Business.Repositories;

public interface IToDoRepository : IRepository
{
    Task<ToDoModel> GetByIdAsync(int id);
    Task<List<ToDoModel>> GetWithCategoryAsync(string? like, ToDosSortOrder sortOrder, int? categoryId);
    Task<ToDoModel> CreateAsync(ToDoModel toDo);
    Task<ToDoModel> UpdateAsync(ToDoModel toDo);
    Task RemoveAsync(int id);

}
