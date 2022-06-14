using GraphQL;
using GraphQL.Types;
using ToDoList.Business.Abstractions;
using ToDoList.Business.Enums;
using ToDoList.GraphQL.EnumTypes;
using ToDoList.Server.GraphQL.Types;

namespace ToDoList.GraphQL.Modules.ToDos
{
    public class ToDosQueries : ObjectGraphType
    {
        private readonly IToDoRepository toDoRepository;

        public ToDosQueries(IHttpContextAccessor httpContextAccessor, IEnumerable<IToDoRepository> toDoRepositories)
        {
            string? dataProvider = httpContextAccessor.HttpContext.Request.Cookies["DataProvider"];
            toDoRepository = toDoRepositories.GetPropered(dataProvider);

            Field<NonNullGraphType<GetEntitiesResponseType<ToDoType, ToDoModel>>, GetEntitiesResponse<ToDoModel>>()
               .Name("Get")
               .Argument<IntGraphType, int>("Page", "Argument for Get ToDos")
               .Argument<StringGraphType, string?>("Like", "Argument for Get ToDos")
               .Argument<ToDosSortOrderType, ToDosSortOrder?>("SortOrder", "Argument for Get ToDos")
               .Argument<IntGraphType, int?>("CategoryId", "Argument for Get ToDos")
               .ResolveAsync(async context =>
               {
                   int page = context.GetArgument<int>("Page");
                   string? like = context.GetArgument<string?>("Like");
                   ToDosSortOrder sortOrder = context.GetArgument<ToDosSortOrder?>("SortOrder") ?? ToDosSortOrder.DeadlineAcs;
                   int? categoryId = context.GetArgument<int?>("CategoryId");
                   return await toDoRepository.GetAsync(page, like, sortOrder, categoryId);
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
