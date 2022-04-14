using Dapper;
using System.Data;

namespace ToDoList.Repositories
{
    public class CategoryRepository : BaseDapperRepository<CategoryModel>
    {
        private readonly IDbConnection _dbConnection;
        public CategoryRepository(IDbConnection dbConnection) : base(dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<CategoryModel> GetByIdWithTodosAsync(int id)
        {
            return (await GetWithTodosAsync()).FirstOrDefault(c => c.Id == id);
        }

        public async Task<List<CategoryModel>> GetWithTodosAsync()
        {
            string toDoTableName = new ToDoModel().GetTableName();
            string query = $"select * from {TableName} " +
                           $"left join {toDoTableName} on {TableName}.id = {toDoTableName}.categoryId";
            var lookup = new Dictionary<int, CategoryModel>();
            return (await _dbConnection.QueryAsync<CategoryModel, ToDoModel, CategoryModel>(query, (c, toDo) =>
            {
                CategoryModel category;
                if (!lookup.TryGetValue(c.Id, out category))
                {
                    lookup.Add(c.Id, category = c);
                }
                if(toDo != null)
                    category.ToDos.Add(toDo);
                return category;
            })).ToList();
        }
    }
}
