using Dapper;
using System.Data;
using ToDoList.Enums;

namespace ToDoList.Repositories
{
    public class ToDoRepository : BaseDapperRepository<ToDoModel>
    {
        private readonly IDbConnection _dbConnection;

        public ToDoRepository(IDbConnection dbConnection) : base(dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public override async Task<ToDoModel> CreateAsync(ToDoModel toDo)
        {
            if (toDo.CategoryId == 0)
                toDo.CategoryId = null;
            return await base.CreateAsync(toDo);
        }

        public override async Task UpdateAsync(ToDoModel toDo)
        {
            if (toDo.CategoryId == 0)
                toDo.CategoryId = null;
            await base.UpdateAsync(toDo);
        }

        public async Task<List<ToDoModel>> GetMyWithCategory(int userId, string? like = null, ToDosSortOrder sortOrder = ToDosSortOrder.DateDesc)
        {
            like = like ?? "";
            like = $"%{like}%";
            string categoryTableName = new CategoryModel().GetTableName();
            string query = $"select * from {TableName} " +
                           $"left join {categoryTableName} on {TableName}.{nameof(ToDoModel.CategoryId)} = {categoryTableName}.{nameof(CategoryModel.Id)} " +
                           $"where {TableName}.{nameof(ToDoModel.Name)} like @like and {TableName}.{nameof(ToDoModel.UserId)} = {userId} ";

            //int total = await _dbConnection.QueryFirstOrDefaultAsync<int>(query.Replace("*", "count(*)"), new { like });

            switch (sortOrder)
            {
                case ToDosSortOrder.NameDesc:
                    query += $"order by {TableName}.{nameof(ToDoModel.Name)} desc";
                    break;
                case ToDosSortOrder.NameAsc:
                    query += $"order by {TableName}.{nameof(ToDoModel.Name)} asc";
                    break;
                case ToDosSortOrder.DeadlineDecs:
                    query += $"order by {TableName}.{nameof(ToDoModel.Deadline)} desc";
                    break;
                case ToDosSortOrder.DeadlineAcs :
                    query += $"order by {TableName}.{nameof(ToDoModel.Deadline)} asc";
                    break;
                case ToDosSortOrder.DateAsc:
                    query += $"order by {TableName}.{nameof(ToDoModel.CreatedAt)} asc";
                    break;
                case ToDosSortOrder.DateDesc:
                    query += $"order by {TableName}.{nameof(ToDoModel.CreatedAt)} desc";
                    break;
            }

            //int offset = PageSize * (page - 1);
            //query += $" offset {offset} rows fetch next {PageSize} rows only";
            var entities = (await _dbConnection.QueryAsync<ToDoModel, CategoryModel, ToDoModel>(query, (toDo, category) =>
            {
                toDo.CategoryId = category?.Id;
                toDo.Category = category;
                return toDo;
            }, new { like })).ToList();
            return entities;
        }
    }
}
