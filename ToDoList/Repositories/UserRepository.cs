using Dapper;
using System.Data;

namespace ToDoList.Repositories
{
    public class UserRepository : BaseDapperRepository<UserModel>
    {
        private readonly IDbConnection _dbConnection;
        public UserRepository(IDbConnection dbConnection) : base(dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<UserModel> GetByEmailAsync(string email)
        {
            string query = $"select * from {TableName} where email = @email";
            return (await _dbConnection.QueryAsync<UserModel>(query, new { email })).FirstOrDefault();
        }
    }
}
