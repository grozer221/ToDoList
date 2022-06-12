using GraphQL;
using GraphQL.Types;
using ToDoList.Business.Abstractions;
using ToDoList.Business.Enums;
using ToDoList.GraphQL.EnumTypes;
using ToDoList.Server.GraphQL.Types;

namespace ToDoList.GraphQL.Modules.Categories
{
    public class CategoriesQueries : ObjectGraphType
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoriesQueries(IHttpContextAccessor httpContextAccessor, IEnumerable<ICategoryRepository> categoryRepositories)
        {
            string? dataProvider = httpContextAccessor.HttpContext.Request.Cookies["DataProvider"];
            categoryRepository = categoryRepositories.GetPropered(dataProvider);

            Field<NonNullGraphType<GetEntitiesResponseType<CategoryType, CategoryModel>>, GetEntitiesResponse<CategoryModel>>()
               .Name("Get")
               .Argument<NonNullGraphType<IntGraphType>, int>("Page", "Argument for Get Categories")
               .Argument<StringGraphType, string?>("Like", "Argument for Get Categories")
               .Argument<CategoriesSortOrderType, CategoriesSortOrder?>("SortOrder", "Argument for Get Categories")
               .ResolveAsync(async context =>
               {
                   int page = context.GetArgument<int>("Page");
                   string? like = context.GetArgument<string?>("Like");
                   CategoriesSortOrder sortOrder = context.GetArgument<CategoriesSortOrder?>("SortOrder") ?? CategoriesSortOrder.DateAsc;
                   return await categoryRepository.GetAsync(like, sortOrder, page);
               });

            Field<NonNullGraphType<CategoryType>, CategoryModel>()
                .Name("GetById")
                .Argument<NonNullGraphType<IntGraphType>, int>("Id", "Argument for Get Category")
                .ResolveAsync(async context =>
                {
                    int id = context.GetArgument<int>("Id");
                    return await categoryRepository.GetByIdAsync(id);
                });
        }
    }
}
