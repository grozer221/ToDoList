using GraphQL.Types;
using ToDoList.GraphQL.Modules.Categories;
using ToDoList.GraphQL.Modules.ToDos;

namespace ToDoList.GraphQL
{
    public class RootQueries : ObjectGraphType
    {
        public RootQueries()
        {
            Field<ToDosQueries>()
                .Name("ToDos")
                .Resolve(_ => new { });
            
            Field<CategoriesQueries>()
                .Name("Categories")
                .Resolve(_ => new { });
        }
    }
}
