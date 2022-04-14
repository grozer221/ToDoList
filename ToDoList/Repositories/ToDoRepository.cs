using Dapper;
using System.Data;

namespace ToDoList.Repositories
{
    public class ToDoRepository : BaseDapperRepository<ToDoModel>
    {
        private readonly IDbConnection _dbConnection;
        public ToDoRepository(IDbConnection dbConnection) : base(dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public override async Task CreateAsync(ToDoModel toDo)
        {
            if (toDo.CategoryId == 0)
                toDo.CategoryId = null;
            await base.CreateAsync(toDo);
        }
        
        public override async Task UpdateAsync(ToDoModel toDo)
        {
            if (toDo.CategoryId == 0)
                toDo.CategoryId = null;
            await base.UpdateAsync(toDo);
        }

        public async Task<List<ToDoModel>> GetWithCategory()
        {
            string categoryTableName = new CategoryModel().GetTableName();
            string query = $"select * from {TableName} " +
                           $"left join {categoryTableName} on {TableName}.categoryId = {categoryTableName}.id";
            return (await _dbConnection.QueryAsync<ToDoModel, CategoryModel, ToDoModel>(query, (toDo, category) =>
            {
                toDo.CategoryId = category?.Id;
                toDo.Category = category;
                return toDo;
            })).ToList();
        }
    }
}
