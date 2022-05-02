using ToDoList.Business.Enums;
using ToDoList.Business.Models;

namespace ToDoList.Business.Repositories;

public interface IToDoRepository
{
    Task<ToDoModel> GetByIdAsync(int id);
    Task<List<ToDoModel>> GetMyWithCategory(int userId, string? like, ToDosSortOrder sortOrder, int? categoryId);
    Task<List<ToDoModel>> GetAsync();
    Task<ToDoModel> CreateAsync(ToDoModel toDo);
    Task<ToDoModel> UpdateAsync(ToDoModel toDo);
    Task RemoveAsync(int id);

}
