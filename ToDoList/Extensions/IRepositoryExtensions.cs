using ToDoList.Enums;
using ToDoList.MsSql.Repositories;
using ToDoList.XML.Repositories;

namespace ToDoList.Extensions
{
    public static class IRepositoryExtensions
    {
        public static T GetPropered<T>(this IEnumerable<T> repositories, DataProvider dataProvider) where T : IRepository
        {
            switch (dataProvider)
            {
                case DataProvider.Xml:
                    if(repositories is IEnumerable<IToDoRepository>)
                    {
                        return repositories.Single(r => r.GetType() == typeof(XmlToDoRepository));
                    }
                    else if (repositories is IEnumerable<ICategoryRepository>)
                    {
                        return repositories.Single(r => r.GetType() == typeof(XmlCategoryRepository));
                    }
                    else
                    {
                        throw new Exception("Bad repositories");
                    }

                case DataProvider.Database:
                default:
                    if (repositories is IEnumerable<IToDoRepository>)
                    {
                        return repositories.Single(r => r.GetType() == typeof(MSSqlToDoRepository));
                    }
                    else if (repositories is IEnumerable<ICategoryRepository>)
                    {
                        return repositories.Single(r => r.GetType() == typeof(MSSqlCategoryRepository));
                    }
                    else
                    {
                        throw new Exception("Bad repositories");
                    }
            }
        }
    }
}
