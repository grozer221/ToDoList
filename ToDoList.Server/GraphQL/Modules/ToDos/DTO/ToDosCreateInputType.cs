using GraphQL.Types;

namespace ToDoList.GraphQL.Modules.ToDos.DTO
{
    public class ToDosCreateInputType : InputObjectGraphType<ToDosCreateInput>
    {
        public ToDosCreateInputType()
        {
            Field<NonNullGraphType<StringGraphType>, string>()
                .Name("Name")
                .Resolve(context => context.Source.Name);
            
            Field<DateTimeGraphType, DateTime?>()
                .Name("Deadline")
                .Resolve(context => context.Source.Deadline);
            
            Field<IntGraphType, int?>()
                .Name("CategoryId")
                .Resolve(context => context.Source.CategoryId);
        }
    }
}
