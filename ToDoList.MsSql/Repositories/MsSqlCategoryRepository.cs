using Dapper;
using System.Data.SqlClient;
using ToDoList.Business.Abstractions;
using ToDoList.Business.Enums;
using ToDoList.Business.Models;
using ToDoList.Business.Repositories;

namespace ToDoList.MsSql.Repositories
{
    public class MSSqlCategoryRepository : ICategoryRepository
    {
        private readonly string connectionString;
        private SqlConnection DbConnection
        {
            get { return new SqlConnection(connectionString); }
        }

        public int Take => 3;

        public MSSqlCategoryRepository(string connectionString)
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

        public async Task<GetEntitiesResponse<CategoryModel>> GetAsync(string? like, CategoriesSortOrder sortOrder, int page)
        {
            like = like ?? "";
            like = $"%{like}%";
            string query = @"select {0} from Categories
                            where Categories.Name like @like ";

            string getCountQuery = string.Format(query, "count(*)");
            string getEntitieQuery = string.Format(query, "*");

            switch (sortOrder)
            {
                case CategoriesSortOrder.NameDesc:
                    getEntitieQuery += $"order by Categories.Name desc";
                    break;
                case CategoriesSortOrder.NameAsc:
                    getEntitieQuery += $"order by Categories.Name asc";
                    break;
                case CategoriesSortOrder.DateAsc:
                    getEntitieQuery += $"order by Categories.CreatedAt asc";
                    break;
                case CategoriesSortOrder.DateDesc:
                    getEntitieQuery += $"order by Categories.CreatedAt desc";
                    break;
            }
            getEntitieQuery += " OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY";
            int skip = (page - 1) * Take;
            using (var connection = DbConnection)
            {
                await connection.OpenAsync();
                var reader = await connection.QueryMultipleAsync($"{getCountQuery} {getEntitieQuery}", new { like, Take, skip });
                int total = reader.Read<int>().FirstOrDefault();
                var categories = reader.Read<CategoryModel>();
                return new GetEntitiesResponse<CategoryModel>
                {
                    Entities = categories,
                    PageSize = Take,
                    Total = total,
                };
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
                             SELECT CAST(SCOPE_IDENTITY() as int);";
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