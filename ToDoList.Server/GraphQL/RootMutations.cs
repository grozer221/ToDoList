using GraphQL.Types;
using ToDoList.GraphQL.Modules.Categories;
using ToDoList.GraphQL.Modules.ToDos;

namespace ToDoList.GraphQL
{
    public class RootMutations : ObjectGraphType
    {
        public RootMutations()
        {
            Field<ToDosMutations>()
                .Name("ToDos")
                .Resolve(_ => new { });
            
            Field<CategoriesMutations>()
                .Name("Categories")
                .Resolve(_ => new { });
        }
    }
}
