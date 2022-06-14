using ToDoList.Business.Abstractions;
using ToDoList.Business.Enums;
using ToDoList.Business.Models;

namespace ToDoList.Business.Repositories;

public interface IToDoRepository
{
    int Take { get; }
    Task<ToDoModel> GetByIdAsync(int id);
    Task<GetEntitiesResponse<ToDoModel>> GetAsync(int page, string? like, ToDosSortOrder sortOrder, int? categoryId);
    Task<ToDoModel> CreateAsync(ToDoModel toDo);
    Task<ToDoModel> UpdateAsync(ToDoModel toDo);
    Task<ToDoModel> RemoveAsync(int id);
}
