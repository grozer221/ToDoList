using Dapper;
using System.Data;

namespace ToDoList.Repositories
{
    public class UserRepository : BaseDapperRepository<UserModel>
    {
        private readonly IDbConnection _dbConnection;
        private readonly IHostEnvironment _hostEnvironment;
        public UserRepository(IDbConnection dbConnection, IHostEnvironment hostEnvironment) : base(dbConnection, hostEnvironment)
        {
            _dbConnection = dbConnection;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<UserModel> GetByEmailAsync(string email)
        {
            string query = $"select * from {TableName} where email = @email";
            return (await _dbConnection.QueryAsync<UserModel>(query, new { email })).FirstOrDefault();
        }
    }
}
