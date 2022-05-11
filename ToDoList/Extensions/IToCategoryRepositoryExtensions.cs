using ToDoList.Enums;
using ToDoList.MsSql.Repositories;
using ToDoList.XML.Repositories;

namespace ToDoList.Extensions
{
    public static class IToCategoryRepositoryExtensions
    {
        public static ICategoryRepository GetPropered(this IEnumerable<ICategoryRepository> repositories, string? dataProviderString)
        {
            DataProvider dataProvider;
            bool isDataProviderValid = Enum.TryParse(dataProviderString, out dataProvider);
            if (!isDataProviderValid)
                dataProvider = DataProvider.Database;

            switch (dataProvider)
            {
                case DataProvider.Xml:
                    return repositories.Single(r => r.GetType() == typeof(XmlCategoryRepository));
                case DataProvider.Database:
                default:
                    return repositories.Single(r => r.GetType() == typeof(MSSqlCategoryRepository));
            }
        }
    }
}
