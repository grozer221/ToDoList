using Dapper;
using MySql.Data.MySqlClient;
using ToDoList.Business.Enums;
using ToDoList.Business.Models;
using ToDoList.Business.Repositories;

namespace ToDoList.MySql.Repositories
{
    public class MySqlToDoRepository : IToDoRepository
    {
        private readonly string connectionString;
        private MySqlConnection DbConnection
        {
            get { return new MySqlConnection(connectionString); }
        }

        public MySqlToDoRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<ToDoModel> GetByIdOrDefaultAsync(int id)
        {
            string query = $"select * from ToDos where id = @id";
            using (var connection = DbConnection)
            {
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<ToDoModel>(query, new { id });
            }
        }

        public async Task<ToDoModel> GetByIdAsync(int id)
        {
            var toDo = await GetByIdOrDefaultAsync(id);
            if (toDo == null)
                throw new Exception($"ToDo with id {id} not found");
            return toDo;
        }

        public async Task<IEnumerable<ToDoModel>> GetWithCategoryOrDefaultAsync(string? like, ToDosSortOrder sortOrder, int? categoryId)
        {
            like = like ?? "";
            like = $"%{like}%";
            string query = @"select * from Todos 
                            left join Categories on ToDos.categoryId = Categories.Id 
                            where ToDos.Name like @like ";

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
                case ToDosSortOrder.DateCompleteAsc:
                    query += GetOrderBy("DateComplete", "asc");
                    break;
                case ToDosSortOrder.DateCompleteDesc:
                    query += GetOrderBy("DateComplete", "desc");
                    break;
            }
            using (var connection = DbConnection)
            {
                await connection.OpenAsync();
                return await connection.QueryAsync<ToDoModel, CategoryModel, ToDoModel>(query, (toDo, category) =>
                {
                    toDo.CategoryId = category?.Id;
                    toDo.Category = category;
                    return toDo;
                }, new { like, categoryId });
            }
        }

        public async Task<IEnumerable<ToDoModel>> GetWithCategoryAsync(string? like = null, ToDosSortOrder sortOrder = ToDosSortOrder.DeadlineAcs, int? categoryId = null)
        {
            var toDos = await GetWithCategoryOrDefaultAsync(like, sortOrder, categoryId);
            if (toDos == null)
                throw new Exception($"ToDos not found");
            return toDos;
        }

        private string GetOrderBy(string columnName, string typeColumnName)
        {
            if (string.Compare(columnName, "Deadline", true) == 0 || string.Compare(columnName, "DateComplete", true) == 0)
            {
                string dateTimeIfNull;
                if (string.Compare(typeColumnName, "desc") == 0)
                    dateTimeIfNull = "1753-01-01 00:00:00";
                else
                    dateTimeIfNull = "9999-12-31 23:59:59";

                return $@"order by ToDos.IsComplete asc, 
                        CASE WHEN ToDos.{columnName} IS NULL THEN '{dateTimeIfNull}' ELSE ToDos.{columnName} END {typeColumnName}";

            }

            return $@"order by ToDos.IsComplete asc, 
                    ToDos.{columnName} {typeColumnName}";
        }

        public async Task<ToDoModel> CreateAsync(ToDoModel toDo)
        {
            if (toDo.CategoryId == 0)
                toDo.CategoryId = null;
            DateTime dateTimeNow = DateTime.Now;
            toDo.CreatedAt = dateTimeNow;
            toDo.UpdatedAt = dateTimeNow;
            string query = $@"insert into Todos 
                        (Name, IsComplete, Deadline, CategoryId, CreatedAt, UpdatedAt) 
                        values (@Name, @IsComplete, @Deadline, @CategoryId, @CreatedAt, @UpdatedAt);
                        SELECT LAST_INSERT_ID();";
            using (var connection = DbConnection)
            {
                await connection.OpenAsync();
                toDo.Id = await connection.QuerySingleAsync<int>(query, toDo);
                return toDo;
            }
        }

        public async Task<ToDoModel> UpdateAsync(ToDoModel toDo)
        {
            ToDoModel toDoIsExists = await GetByIdAsync(toDo.Id);
            if (toDoIsExists == null)
                throw new Exception($"ToDo with id {toDo.Id} does not exists");

            string query = @"update ToDos 
                            set Name = @Name, IsComplete = @IsComplete, DateComplete = @DateComplete, Deadline = @Deadline, CategoryId = @CategoryId, UpdatedAt = @UpdatedAt 
                            where id = @id";

            if (toDo.CategoryId == 0)
                toDo.CategoryId = null;

            DateTime dateTimeNow = DateTime.Now;
            if (!toDoIsExists.IsComplete && toDo.IsComplete)
                toDo.DateComplete = dateTimeNow;
            else if (toDoIsExists.IsComplete && !toDo.IsComplete)
                toDo.DateComplete = null;
            else
                toDo.DateComplete = toDoIsExists.DateComplete;

            toDo.UpdatedAt = dateTimeNow;
            using (var connection = DbConnection)
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(query, toDo);
                return toDo;
            }
        }

        public async Task<ToDoModel> RemoveAsync(int id)
        {
            ToDoModel toDoIsExists = await GetByIdAsync(id);
            if (toDoIsExists == null)
                throw new Exception($"ToDo with id {id} does not exists");

            string query = $"delete from ToDos where id = @id";
            using (var connection = DbConnection)
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(query, new { id });
            }
            return toDoIsExists;
        }
    }
}
