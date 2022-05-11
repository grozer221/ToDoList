using AutoMapper;
using GraphQL;
using GraphQL.Types;
using ToDoList.GraphQL.Modules.ToDos.DTO;

namespace ToDoList.GraphQL.Modules.ToDos
{
    public class ToDosMutations : ObjectGraphType
    {
        private readonly IToDoRepository toDoRepository;

        public ToDosMutations(IEnumerable<IToDoRepository> toDoRepositories, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            string? dataProvider = httpContextAccessor.HttpContext.Request.Cookies["DataProvider"];
            toDoRepository = toDoRepositories.GetPropered(dataProvider);

            Field<ToDoType, ToDoModel>()
                .Name("Create")
                .Argument<NonNullGraphType<ToDosCreateInputType>, ToDosCreateInput>("ToDosCreateInputType", "argument for Create ToDo")
                .ResolveAsync(async context =>
                {
                    var toDosCreateInput = context.GetArgument<ToDosCreateInput>("ToDosCreateInputType");
                    new ToDosCreateInputValidator().ValidateAndThrow(toDosCreateInput);
                    var toDoModel = mapper.Map<ToDoModel>(toDosCreateInput);
                    return await toDoRepository.CreateAsync(toDoModel);
                });
            
            Field<ToDoType, ToDoModel>()
                .Name("Update")
                .Argument<NonNullGraphType<ToDosUpdateInputType>, ToDosUpdateInput>("ToDosUpdateInputType", "Argument for Create ToDo")
                .ResolveAsync(async context => 
                {
                    var toDosUpdateInput = context.GetArgument<ToDosUpdateInput>("ToDosUpdateInputType");
                    var toDoModel = mapper.Map<ToDoModel>(toDosUpdateInput);
                    return await toDoRepository.UpdateAsync(toDoModel);
                });
            
            Field<ToDoType, ToDoModel>()
                .Name("Remove")
                .Argument<NonNullGraphType<IntGraphType>, int>("Id", "Argument for Remove ToDo")
                .ResolveAsync(async context => 
                {
                    int id = context.GetArgument<int>("Id");
                    return await toDoRepository.RemoveAsync(id);
                });
        }
    }
}
