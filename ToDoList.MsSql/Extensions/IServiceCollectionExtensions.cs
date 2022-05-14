using Microsoft.Extensions.DependencyInjection;
using ToDoList.Business.Repositories;
using ToDoList.MsSql.Repositories;

namespace ToDoList.MsSql.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddMsSqlDataProvider(this IServiceCollection services, string connectionString)
        {
            services.AddTransient<IToDoRepository>(_ => new MSSqlToDoRepository(connectionString));
            services.AddTransient<ICategoryRepository>(_ => new MSSqlCategoryRepository(connectionString));
            return services;
        }
    }
}
