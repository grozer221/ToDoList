using Dapper;
using System.Data;
using ToDoList.Enums;

namespace ToDoList.Repositories
{
    public class CategoryRepository : BaseDapperRepository<CategoryModel>
    {
        private readonly IDbConnection _dbConnection;
        public CategoryRepository(IDbConnection dbConnection) : base(dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<List<CategoryModel>> GetMyAsync(int userId, string? like, CategoriesSortOrder sortOrder)
        {
            like = like ?? "";
            like = $"%{like}%";
            string query = $"select * from {TableName} " +
                           $"where {TableName}.{nameof(CategoryModel.Name)} like @like and {TableName}.{nameof(CategoryModel.UserId)} = {userId} ";

            switch (sortOrder)
            {
                case CategoriesSortOrder.NameDesc:
                    query += $"order by {TableName}.{nameof(CategoryModel.Name)} desc";
                    break;
                case CategoriesSortOrder.NameAsc:
                    query += $"order by {TableName}.{nameof(CategoryModel.Name)} asc";
                    break;
                case CategoriesSortOrder.DateAsc:
                    query += $"order by {TableName}.{nameof(CategoryModel.CreatedAt)} asc";
                    break;
                case CategoriesSortOrder.DateDesc:
                    query += $"order by {TableName}.{nameof(CategoryModel.CreatedAt)} desc";
                    break;
            }

            return (await _dbConnection.QueryAsync<CategoryModel>(query, new { like })).ToList();
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
