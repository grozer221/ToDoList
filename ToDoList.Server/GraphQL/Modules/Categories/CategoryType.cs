using GraphQL.Types;
using ToDoList.Business.Abstractions;
using ToDoList.GraphQL.Modules.ToDos;
using ToDoList.Server.GraphQL.Types;

namespace ToDoList.GraphQL.Modules.Categories
{
    public class CategoryType : ObjectGraphType<CategoryModel>
    {
        private readonly IToDoRepository toDoRepository;
        public CategoryType(IEnumerable<IToDoRepository> toDoRepositories, IHttpContextAccessor httpContextAccessor) 
        {
            string? dataProvider = httpContextAccessor.HttpContext.Request.Cookies["DataProvider"];
            toDoRepository = toDoRepositories.GetPropered(dataProvider);

            Field<NonNullGraphType<IntGraphType>, int>()
               .Name("Id")
               .Resolve(context => context.Source.Id);
            
            Field<NonNullGraphType<StringGraphType>, string>()
               .Name("Name")
               .Resolve(context => context.Source.Name);
            
            Field<NonNullGraphType<DateTimeGraphType>, DateTime>()
               .Name("CreatedAt")
               .Resolve(context => context.Source.CreatedAt);
            
            Field<NonNullGraphType<DateTimeGraphType>, DateTime>()
               .Name("UpdatedAt")
               .Resolve(context => context.Source.UpdatedAt);
        }
    }
}
