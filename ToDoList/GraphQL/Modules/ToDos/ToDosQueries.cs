using GraphQL;
using GraphQL.Types;
using ToDoList.Business.Enums;

namespace ToDoList.GraphQL.Modules.ToDos
{
    public class ToDosQueries : ObjectGraphType
    {
        private readonly IToDoRepository toDoRepository;

        public ToDosQueries(IHttpContextAccessor httpContextAccessor, IEnumerable<IToDoRepository> toDoRepositories)
        {
            string? dataProvider = httpContextAccessor.HttpContext.Request.Cookies["DataProvider"];
            toDoRepository = toDoRepositories.GetPropered(dataProvider);

            Field<NonNullGraphType<ListGraphType<ToDoType>>, IEnumerable<ToDoModel>>()
               .Name("GetAll")
               .ResolveAsync(async context =>
               {
                   return await toDoRepository.GetWithCategoryAsync("", ToDosSortOrder.DeadlineAcs, null);
               });

            Field<NonNullGraphType<ToDoType>, ToDoModel>()
                .Name("GetById")
                .Argument<NonNullGraphType<IntGraphType>, int>("Id", "Argument for Get ToDo")
                .ResolveAsync(async context =>
                {
                    int id = context.GetArgument<int>("Id");
                    return await toDoRepository.GetByIdAsync(id);
                });
        }
    }
}
