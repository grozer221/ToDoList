using Dapper;
using System.Data;

namespace ToDoList.Repositories
{
    public class UserRepository
    {
        private readonly IDbConnection _dbConnection;
        private readonly IHostEnvironment _hostEnvironment;
        public UserRepository(IDbConnection dbConnection, IHostEnvironment hostEnvironment)
        {
            _dbConnection = dbConnection;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<UserModel> GetByEmailAsync(string email)
        {
            string query = @"select * from Users 
                            where email = @email";
            return (await _dbConnection.QueryAsync<UserModel>(query, new { email })).FirstOrDefault();
        }

        public async Task<UserModel> GetByIdAsync(int id)
        {
            string query = @"select * from Users 
                            where id = @id";
            return await _dbConnection.QueryFirstOrDefaultAsync<UserModel>(query, new { id });
        }

        public async Task<List<UserModel>> GetAsync()
        {
            string query = $"select * from Users";
            return (await _dbConnection.QueryAsync<UserModel>(query)).ToList();
        }

        public async Task<UserModel> CreateAsync(UserModel user)
        {
            DateTime dateTimeNow = DateTime.Now;
            user.CreatedAt = dateTimeNow;
            user.UpdatedAt = dateTimeNow;
            string query = $@"insert into Todos 
                        (Email, Password, IsEmailConfirmed) 
                        values (@Email, @Password, @IsEmailConfirmed);";
            query += _hostEnvironment.IsDevelopment() ? "SELECT CAST(SCOPE_IDENTITY() as int);" : "SELECT LAST_INSERT_ID();";
            user.Id = await _dbConnection.QuerySingleAsync<int>(query, user);
            return user;
        }

        public async Task<UserModel> UpdateAsync(UserModel user)
        {
            UserModel userIsExists = await GetByIdAsync(user.Id);
            if (userIsExists == null)
                throw new Exception($"ToDo with id {user.Id} does not exists");

            string query = @"update Users 
                            set Email = @Email, Password = @Password, IsEmailConfirmed = @IsEmailConfirmed 
                            where id = @id";
            user.UpdatedAt = DateTime.Now;
            await _dbConnection.ExecuteAsync(query, user);
            return user;
        }

        public async Task RemoveAsync(int id)
        {
            UserModel userIsExists = await GetByIdAsync(id);
            if (userIsExists == null)
                throw new Exception($"User with id {id} does not exists");

            string query = @"delete from Users 
                            where id = @id";
            await _dbConnection.ExecuteAsync(query, new { id });
        }
    }
}
