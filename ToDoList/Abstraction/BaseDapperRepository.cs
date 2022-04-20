﻿using Dapper;
using Pluralize.NET.Core;
using System.Data;

namespace ToDoList.Abstraction
{
    public class BaseDapperRepository<T> : IBaseRepository<T> where T : BaseModel
    {
        private readonly IDbConnection _dbConnection;
        private readonly IHostEnvironment _hostEnvironment;
        public static readonly string ModelNameWithrouSuffix = nameof(T).ReplaceInEnd("Model", string.Empty);
        public static readonly string TableName = new Pluralizer().Pluralize(ModelNameWithrouSuffix);

        public int PageSize { get => 2; }

        public BaseDapperRepository(IDbConnection dbConnection, IHostEnvironment hostEnvironment)
        {
            _dbConnection = dbConnection;
            _hostEnvironment = hostEnvironment;
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            string query = $"select * from {TableName} where id = @id";
            return await _dbConnection.QueryFirstOrDefaultAsync<T>(query, new { id });
        }

        public virtual async Task<List<T>> GetAsync()
        {
            string query = $"select * from {TableName}";
            return (await _dbConnection.QueryAsync<T>(query)).ToList();
        }
        
        public virtual async Task<T> CreateAsync(T baseModel)
        {
            DateTime dateTimeNow = DateTime.Now;
            baseModel.CreatedAt = dateTimeNow;
            baseModel.UpdatedAt = dateTimeNow;
            var columns = GetColumns();
            var stringOfColumns = string.Join(", ", columns);
            var stringOfParameters = string.Join(", ", columns.Select(c => "@" + c));
            string query = $"insert into {TableName} ({stringOfColumns}) values ({stringOfParameters});";
            query += _hostEnvironment.IsDevelopment() ? "SELECT CAST(SCOPE_IDENTITY() as int);" : "SELECT LAST_INSERT_ID();";
            baseModel.Id = await _dbConnection.QuerySingleAsync<int>(query, baseModel);
            return baseModel;
        }

        public virtual async Task UpdateAsync(T baseModel)
        {
            T baseModelIsExists = await GetByIdAsync(baseModel.Id);
            if (baseModelIsExists == null)
                throw new Exception($"{ModelNameWithrouSuffix} with id {baseModel.Id} does not exists");

            var columns = GetColumns().Where(c => c != "CreatedAt");
            var stringOfColumns = string.Join(", ", columns.Select(e => $"{e} = @{e}"));
            string query = $"update {TableName} set {stringOfColumns} where id = @id";
            baseModel.UpdatedAt = DateTime.Now;
            await _dbConnection.ExecuteAsync(query, baseModel);
        }

        public virtual async Task RemoveAsync(int id)
        {
            T baseModelIsExists = await GetByIdAsync(id);
            if (baseModelIsExists == null)
                throw new Exception($"{ModelNameWithrouSuffix} with id {id} does not exists");

            string query = $"delete from {TableName} where id = @id";
            await _dbConnection.ExecuteAsync(query, new { id });
        }

        private IEnumerable<string> GetColumns()
        {
            return typeof(T)
                .GetProperties()
                .Where(p => p.Name != nameof(BaseModel.Id) && (p.PropertyType.IsValueType || p.PropertyType == typeof(string)))
                .Select(p => p.Name);
        }
    }
}
