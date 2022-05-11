using GraphQL.Types;

namespace ToDoList.GraphQL.Modules.Categories.DTO
{
    public class CategoriesCreateInputType : InputObjectGraphType<CategoriesCreateInput>
    {
        public CategoriesCreateInputType()
        {
            Field<NonNullGraphType<StringGraphType>, string>()
                .Name("Name")
                .Resolve(context => context.Source.Name);
        }
    }
}
