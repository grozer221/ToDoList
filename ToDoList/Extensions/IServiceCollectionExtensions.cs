using GraphQL;
using GraphQL.Server;
using ToDoList.GraphQL;

namespace ToDoList.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddGraphQLApi(this IServiceCollection services)
        {
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
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
                .AddGraphTypes(typeof(AppSchema));
            return services;
        }
    }
}
