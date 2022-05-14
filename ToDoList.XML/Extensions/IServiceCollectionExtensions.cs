using Microsoft.Extensions.DependencyInjection;
using ToDoList.Business.Repositories;
using ToDoList.XML.Repositories;

namespace ToDoList.XML.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddXmlDataProdiver(this IServiceCollection services, string fileName)
        {
            services.AddTransient<IToDoRepository>(_ => new XmlToDoRepository(fileName));
            services.AddTransient<ICategoryRepository>(_ => new XmlCategoryRepository(fileName));
            return services;
        }
    }
}
