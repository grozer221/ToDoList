namespace ToDoList.Repositories.Abstraction
{
    public interface IUserRepository
    {
        Task<UserModel> GetByEmailAsync(string email);
        Task<UserModel> GetByIdAsync(int id);
        Task<List<UserModel>> GetAsync();
        Task<UserModel> CreateAsync(UserModel user);
        Task<UserModel> UpdateAsync(UserModel user);
        Task RemoveAsync(int id);
    }
}
