using GraphQL;
using GraphQL.Types;
using ToDoList.Business.Enums;

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
               .ResolveAsync(async context =>
               {
                   return await categoryRepository.GetAsync("", CategoriesSortOrder.DateAsc);
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
