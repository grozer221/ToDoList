using Dapper;
using MySql.Data.MySqlClient;
using ToDoList.Business.Enums;
using ToDoList.Business.Models;
using ToDoList.Business.Repositories;

namespace ToDoList.MySql.Repositories
{
    public class MySqlCategoryRepository : ICategoryRepository
    {
        private readonly string connectionString;
        private MySqlConnection DbConnection
        {
            get { return new MySqlConnection(connectionString); }
        }

        public MySqlCategoryRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<CategoryModel> GetByIdOrDefaultAsync(int id)
        {
            string query = $"select * from Categories where id = @id";
            using (var connection = DbConnection)
            {
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<CategoryModel>(query, new { id });
            }
        }

        public async Task<CategoryModel> GetByIdAsync(int id)
        {
            var category = await GetByIdOrDefaultAsync(id);
            if (category == null)
                throw new Exception($"Category with id {id} not found");
            return category;
        }

        public async Task<IEnumerable<CategoryModel>> GetOrDefaultAsync(string? like, CategoriesSortOrder sortOrder)
        {
            like = like ?? "";
            like = $"%{like}%";
            string query = @"select * from Categories
                            where Categories.Name like @like ";

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
            using (var connection = DbConnection)
            {
                await connection.OpenAsync();
                return await connection.QueryAsync<CategoryModel>(query, new { like });
            }
        }

        public async Task<IEnumerable<CategoryModel>> GetAsync(string? like, CategoriesSortOrder sortOrder)
        {
            var categories = await GetOrDefaultAsync(like, sortOrder);
            if (categories == null)
                throw new Exception($"Categories not found");
            return categories;
        }

        public async Task<CategoryModel> GetByIdWithTodosOrDefaultAsync(int id)
        {
            var categories = await GetWithTodosAsync();
            return categories.FirstOrDefault(c => c.Id == id);
        }

        public async Task<CategoryModel> GetByIdWithTodosAsync(int id)
        {
            var category = await GetByIdWithTodosAsync(id);
            if (category == null)
                throw new Exception($"Category with id {id} not found");
            return category;
        }

        public async Task<IEnumerable<CategoryModel>> GetWithTodosAsync()
        {
            string query = @"select * from Categories 
                            left join ToDos on Categories.Id = ToDos.CategoryId";
            var lookup = new Dictionary<int, CategoryModel>();
            using (var connection = DbConnection)
            {
                await connection.OpenAsync();
                return await connection.QueryAsync<CategoryModel, ToDoModel, CategoryModel>(query, (c, toDo) =>
                {
                    CategoryModel category;
                    if (!lookup.TryGetValue(c.Id, out category))
                    {
                        lookup.Add(c.Id, category = c);
                    }
                    if (toDo != null)
                        category.ToDos.Add(toDo);
                    return category;
                });
            }
        }

        public async Task<CategoryModel> CreateAsync(CategoryModel category)
        {
            DateTime dateTimeNow = DateTime.Now;
            category.CreatedAt = dateTimeNow;
            category.UpdatedAt = dateTimeNow;
            string query = $@"insert into Categories 
                            (Name, CreatedAt, UpdatedAt) 
                            values (@Name, @CreatedAt, @UpdatedAt);
                            SELECT LAST_INSERT_ID();";

            using (var connection = DbConnection)
            {
                await connection.OpenAsync();
                category.Id = await connection.QuerySingleAsync<int>(query, category);
                return category;
            }
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
            using (var connection = DbConnection)
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(query, category);
                return category;
            }
        }

        public async Task<CategoryModel> RemoveAsync(int id)
        {
            CategoryModel categoryIsExists = await GetByIdAsync(id);
            if (categoryIsExists == null)
                throw new Exception($"Category with id {id} does not exists");

            string query = @"delete from Categories 
                            where id = @id";
            using (var connection = DbConnection)
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(query, new { id });
            }
            return categoryIsExists;
        }
    }
}
