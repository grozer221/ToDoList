using Microsoft.Extensions.DependencyInjection;
using ToDoList.Business.Repositories;
using ToDoList.MySql.Repositories;

namespace ToDoList.MySql.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddMySqlDataProvider(this IServiceCollection services, string connectionString)
        {
            connectionString = AppDbContext.ConvertMySqlConnectionString(connectionString);
            services.AddTransient<IToDoRepository>(_ => new MySqlToDoRepository(connectionString));
            services.AddTransient<ICategoryRepository>(_ => new MySqlCategoryRepository(connectionString));
            return services;
        }
    }
}
