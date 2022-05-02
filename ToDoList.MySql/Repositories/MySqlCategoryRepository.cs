using Dapper;
using System.Data;
using ToDoList.Business.Enums;
using ToDoList.Business.Models;
using ToDoList.Business.Repositories;

namespace ToDoList.MySql.Repositories
{
    public class MySqlCategoryRepository : ICategoryRepository
    {
        private readonly IDbConnection _dbConnection;
        public MySqlCategoryRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<CategoryModel> GetByIdAsync(int id)
        {
            string query = $"select * from Categories where id = @id";
            return await _dbConnection.QueryFirstOrDefaultAsync<CategoryModel>(query, new { id });
        }

        public async Task<List<CategoryModel>> GetAsync()
        {
            string query = $"select * from Categories";
            return (await _dbConnection.QueryAsync<CategoryModel>(query)).ToList();
        }

        public async Task<List<CategoryModel>> GetMyAsync(int userId, string? like, CategoriesSortOrder sortOrder)
        {
            like = like ?? "";
            like = $"%{like}%";
            string query = @"select * from Categories
                            where Categories.Name like @like and Categories.UserId = @userId ";

            switch (sortOrder)
            {
                case CategoriesSortOrder.NameDesc:
                    query += $"order by Categories.Name desc";
                    break;
                case CategoriesSortOrder.NameAsc:
                    query += $"order by Categories.Name asc";
                    break;
                case CategoriesSortOrder.DateAsc:
                    query += $"order by Categories.CreatedAt asc";
                    break;
                case CategoriesSortOrder.DateDesc:
                    query += $"order by Categories.CreatedAt desc";
                    break;
            }

            return (await _dbConnection.QueryAsync<CategoryModel>(query, new { like, userId })).ToList();
        }

        public async Task<CategoryModel> GetByIdWithTodosAsync(int id)
        {
            return (await GetWithTodosAsync()).FirstOrDefault(c => c.Id == id);
        }

        public async Task<List<CategoryModel>> GetWithTodosAsync()
        {
            string query = @"select * from Categories 
                            left join ToDos on Categories.Id = ToDos.CategoryId";
            var lookup = new Dictionary<int, CategoryModel>();
            return (await _dbConnection.QueryAsync<CategoryModel, ToDoModel, CategoryModel>(query, (c, toDo) =>
            {
                CategoryModel category;
                if (!lookup.TryGetValue(c.Id, out category))
                {
                    lookup.Add(c.Id, category = c);
                }
                if (toDo != null)
                    category.ToDos.Add(toDo);
                return category;
            })).ToList();
        }

        public async Task<CategoryModel> CreateAsync(CategoryModel category)
        {
            DateTime dateTimeNow = DateTime.Now;
            category.CreatedAt = dateTimeNow;
            category.UpdatedAt = dateTimeNow;
            string query = $@"insert into Categories 
                            (Name, UserId, CreatedAt, UpdatedAt) 
                            values (@Name, @UserId, @CreatedAt, @UpdatedAt);
                            SELECT LAST_INSERT_ID();";
            category.Id = await _dbConnection.QuerySingleAsync<int>(query, category);
            return category;
        }

        public async Task<CategoryModel> UpdateAsync(CategoryModel category)
        {
            CategoryModel categoryIsExists = await GetByIdAsync(category.Id);
            if (categoryIsExists == null)
                throw new Exception($"category with id {category.Id} does not exists");

            string query = @"update Categories 
                            set Name = @Name, UpdatedAt = @UpdatedAt
                            where id = @id";
            category.UpdatedAt = DateTime.Now;
            await _dbConnection.ExecuteAsync(query, category);
            return category;
        }

        public async Task RemoveAsync(int id)
        {
            CategoryModel categoryIsExists = await GetByIdAsync(id);
            if (categoryIsExists == null)
                throw new Exception($"Category with id {id} does not exists");

            string query = @"delete from Categories 
                            where id = @id";
            await _dbConnection.ExecuteAsync(query, new { id });
        }
    }
}
