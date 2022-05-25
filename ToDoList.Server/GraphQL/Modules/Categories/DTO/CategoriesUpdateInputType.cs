using GraphQL.Types;

namespace ToDoList.GraphQL.Modules.Categories.DTO
{
    public class CategoriesUpdateInputType : InputObjectGraphType<CategoriesUpdateInput>
    {
        public CategoriesUpdateInputType()
        {
            Field<NonNullGraphType<IntGraphType>, int>()
                .Name("Id")
                .Resolve(context => context.Source.Id);
            
            Field<NonNullGraphType<StringGraphType>, string>()
                .Name("Name")
                .Resolve(context => context.Source.Name);
        }
    }
}
