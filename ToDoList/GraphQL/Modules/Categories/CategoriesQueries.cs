using GraphQL;
using GraphQL.Types;
using ToDoList.Business.Enums;
using ToDoList.GraphQL.EnumTypes;

namespace ToDoList.GraphQL.Modules.Categories
{
    public class CategoriesQueries : ObjectGraphType
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoriesQueries(IHttpContextAccessor httpContextAccessor, IEnumerable<ICategoryRepository> categoryRepositories)
        {
            string? dataProvider = httpContextAccessor.HttpContext.Request.Cookies["DataProvider"];
            categoryRepository = categoryRepositories.GetPropered(dataProvider);

            Field<NonNullGraphType<ListGraphType<CategoryType>>, IEnumerable<CategoryModel>>()
               .Name("GetAll")
               .Argument<StringGraphType, string?>("Like", "Argument for Get Categories")
               .Argument<CategoriesSortOrderType, CategoriesSortOrder?>("SortOrder", "Argument for Get Categories")
               .ResolveAsync(async context =>
               {
                   string? like = context.GetArgument<string?>("Like");
                   CategoriesSortOrder sortOrder = context.GetArgument<CategoriesSortOrder?>("SortOrder") ?? CategoriesSortOrder.DateAsc;
                   return await categoryRepository.GetAsync(like, sortOrder);
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
