using Dapper;
using System.Data;
using ToDoList.Enums;

namespace ToDoList.Repositories
{
    public class ToDoRepository
    {
        private readonly IDbConnection _dbConnection;
        private readonly IHostEnvironment _hostEnvironment;

        public ToDoRepository(IDbConnection dbConnection, IHostEnvironment hostEnvironment)
        {
            _dbConnection = dbConnection;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<ToDoModel> GetByIdAsync(int id)
        {
            string query = $"select * from ToDos where id = @id";
            return await _dbConnection.QueryFirstOrDefaultAsync<ToDoModel>(query, new { id });
        }

        public async Task<List<ToDoModel>> GetAsync()
        {
            string query = $"select * from ToDos";
            return (await _dbConnection.QueryAsync<ToDoModel>(query)).ToList();
        }

        public async Task<ToDoModel> CreateAsync(ToDoModel toDo)
        {
            if (toDo.CategoryId == 0)
                toDo.CategoryId = null;
            DateTime dateTimeNow = DateTime.Now;
            toDo.CreatedAt = dateTimeNow;
            toDo.UpdatedAt = dateTimeNow;
            string query = $@"insert into Todos 
                        (Name, IsDone, Deadline, CategoryId, UserId, CreatedAt, UpdatedAt) 
                        values (@Name, @IsDone, @Deadline, @CategoryId, @UserId, @CreatedAt, @UpdatedAt);";
            query += _hostEnvironment.IsDevelopment() ? "SELECT CAST(SCOPE_IDENTITY() as int);" : "SELECT LAST_INSERT_ID();";
            toDo.Id = await _dbConnection.QuerySingleAsync<int>(query, toDo);
            return toDo;
        }

        public async Task<ToDoModel> UpdateAsync(ToDoModel toDo)
        {
            ToDoModel toDoIsExists = await GetByIdAsync(toDo.Id);
            if (toDoIsExists == null)
                throw new Exception($"ToDo with id {toDo.Id} does not exists");

            if (toDo.CategoryId == 0)
                toDo.CategoryId = null;

            string query = @"update ToDos 
                            set Name = @Name, IsDone = @IsDone, Deadline = @Deadline, CategoryId = @CategoryId, UpdatedAt = @UpdatedAt 
                            where id = @id";
            toDo.UpdatedAt = DateTime.Now;
            await _dbConnection.ExecuteAsync(query, toDo);
            return toDo;
        }

        public async Task RemoveAsync(int id)
        {
            ToDoModel toDoIsExists = await GetByIdAsync(id);
            if (toDoIsExists == null)
                throw new Exception($"ToDo with id {id} does not exists");

            string query = $"delete from ToDos where id = @id";
            await _dbConnection.ExecuteAsync(query, new { id });
        }

        public async Task<List<ToDoModel>> GetMyWithCategory(int userId, string? like = null, ToDosSortOrder sortOrder = ToDosSortOrder.DeadlineAcs, int? categoryId = null)
        {
            like = like ?? "";
            like = $"%{like}%";
            string query = @"select * from Todos 
                            left join Categories on ToDos.categoryId = Categories.Id 
                            where ToDos.Name like @like and ToDos.UserId = @userId ";

            if (categoryId != null && categoryId != 0)
                query += @"and ToDos.CategoryId = @categoryId ";

            switch (sortOrder)
            {
                case ToDosSortOrder.DeadlineAcs:
                    query += GetOrderBy("Deadline", "asc");
                    break;
                case ToDosSortOrder.DeadlineDecs:
                    query += GetOrderBy("Deadline", "desc");
                    break;
                case ToDosSortOrder.NameAsc:
                    query += GetOrderBy("Name", "asc");
                    break;
                case ToDosSortOrder.NameDesc:
                    query += GetOrderBy("Name", "desc");
                    break;
                case ToDosSortOrder.DateAsc:
                    query += GetOrderBy("CreatedAt", "asc");
                    break;
                case ToDosSortOrder.DateDesc:
                    query += GetOrderBy("CreatedAt", "desc");
                    break;
            }

            return (await _dbConnection.QueryAsync<ToDoModel, CategoryModel, ToDoModel>(query, (toDo, category) =>
            {
                toDo.CategoryId = category?.Id;
                toDo.Category = category;
                return toDo;
            }, new { like, userId, categoryId })).ToList(); ;
        }

        private string GetOrderBy(string columnName, string typeColumnName)
        {
            string dateTimeIfNull;
            if (string.Compare(typeColumnName, "desc") == 0)
                dateTimeIfNull = "1900-01-01 00:00:00";
            else
                dateTimeIfNull = "9999-01-01 00:00:00";
            if (string.Compare(columnName, "Deadline", true) == 0)
                return $@"order by ToDos.IsDone asc, 
                        CASE WHEN ToDos.{columnName} IS NULL THEN '{dateTimeIfNull}' ELSE ToDos.{columnName} END {typeColumnName}";
            return $@"order by ToDos.IsDone asc, 
                    ToDos.{columnName} {typeColumnName}";
        }
    }
}
