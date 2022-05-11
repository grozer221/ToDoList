using GraphQL.Types;

namespace ToDoList.GraphQL.Modules.ToDos.DTO
{
    public class ToDosUpdateInputType : InputObjectGraphType<ToDosUpdateInput>
    {
        public ToDosUpdateInputType()
        {
            Field<NonNullGraphType<IntGraphType>, int>()
                .Name("Id")
                .Resolve(context => context.Source.Id);
            
            Field<NonNullGraphType<BooleanGraphType>, bool>()
                .Name("IsComplete")
                .Resolve(context => context.Source.IsComplete);
            
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
