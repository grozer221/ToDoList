using ToDoList.Business.Enums;
using ToDoList.Business.Models;

namespace ToDoList.Business.Repositories;

public interface IToDoRepository
{
    Task<ToDoModel> GetByIdAsync(int id);
    Task<IEnumerable<ToDoModel>> GetWithCategoryAsync(string? like, ToDosSortOrder sortOrder, int? categoryId);
    Task<ToDoModel> CreateAsync(ToDoModel toDo);
    Task<ToDoModel> UpdateAsync(ToDoModel toDo);
    Task<ToDoModel> RemoveAsync(int id);
}
