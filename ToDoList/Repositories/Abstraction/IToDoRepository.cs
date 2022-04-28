using ToDoList.Enums;

namespace ToDoList.Repositories.Abstraction
{
    public interface IToDoRepository
    {
        Task<ToDoModel> GetByIdAsync(int id);
        Task<List<ToDoModel>> GetAsync();
        Task<ToDoModel> CreateAsync(ToDoModel toDo);
        Task<ToDoModel> UpdateAsync(ToDoModel toDo);
        Task RemoveAsync(int id);
        Task<List<ToDoModel>> GetMyWithCategory(int userId, string? like, ToDosSortOrder sortOrder, int? categoryId);

    }
}
