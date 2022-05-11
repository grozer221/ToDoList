using GraphQL;
using GraphQL.Server;
using ToDoList.GraphQL;
using ToDoList.MsSql.Repositories;
using ToDoList.MySql.Repositories;
using ToDoList.XML.Repositories;

namespace ToDoList.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddMsSqlDataProvider(this IServiceCollection services, string connectionString)
        {
            services.AddTransient<IToDoRepository>(_ => new MSSqlToDoRepository(connectionString));
            services.AddTransient<ICategoryRepository>(_ => new MSSqlCategoryRepository(connectionString));
            return services;
        }
        
        public static IServiceCollection AddMySqlDataProvider(this IServiceCollection services, string connectionString)
        {
            connectionString = AppDbContext.ConvertMySqlConnectionString(connectionString);
            services.AddTransient<IToDoRepository>(_ => new MySqlToDoRepository(connectionString));
            services.AddTransient<ICategoryRepository>(_ => new MySqlCategoryRepository(connectionString));
            return services;
        }
        
        public static IServiceCollection AddXmlDataProdiver(this IServiceCollection services, string fileName)
        {
            services.AddTransient<IToDoRepository>(_ => new XmlToDoRepository(fileName));
            services.AddTransient<ICategoryRepository>(_ => new XmlCategoryRepository(fileName));
            return services;
        }

        public static IServiceCollection AddGraphQLApi(this IServiceCollection services)
        {
            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();

            services.AddSingleton<AppSchema>();
            services
                .AddGraphQL(options =>
                {
                    options.EnableMetrics = true;
                    options.UnhandledExceptionDelegate = (context) =>
                    {
                        Console.WriteLine("StackTrace:" + context.Exception.StackTrace);
                        Console.WriteLine("Message:" + context.Exception.Message);
                        context.ErrorMessage = context.Exception.Message;
                    };
                })
                .AddSystemTextJson()
                .AddWebSockets()
                .AddDataLoader()
                .AddGraphTypes(typeof(AppSchema));
            return services;
        }
    }
}
