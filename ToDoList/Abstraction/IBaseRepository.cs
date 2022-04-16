namespace ToDoList.Abstraction
{
    public interface IBaseRepository<T> where T : BaseModel
    {
        Task<T> GetByIdAsync(int id);
        Task<List<T>> GetAsync();
        Task<T> CreateAsync(T baseModel);
        Task UpdateAsync(T baseModel);
        Task RemoveAsync(int baseModelId);
    }
}
