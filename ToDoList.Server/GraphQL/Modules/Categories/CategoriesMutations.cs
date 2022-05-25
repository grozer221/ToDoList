using AutoMapper;
using GraphQL;
using GraphQL.Types;
using ToDoList.GraphQL.Modules.Categories.DTO;

namespace ToDoList.GraphQL.Modules.Categories
{
    public class CategoriesMutations : ObjectGraphType
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoriesMutations(IEnumerable<ICategoryRepository> categoryRepositories, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            string? dataProvider = httpContextAccessor.HttpContext.Request.Cookies["DataProvider"];
            categoryRepository = categoryRepositories.GetPropered(dataProvider);

            Field<CategoryType, CategoryModel>()
                .Name("Create")
                .Argument<NonNullGraphType<CategoriesCreateInputType>, CategoriesCreateInput>("CategoriesCreateInputType", "Argument for Create Category")
                .ResolveAsync(async context => 
                {
                    var categoriesCreateInput = context.GetArgument<CategoriesCreateInput>("CategoriesCreateInputType");
                    new CategoriesCreateInputValidator().ValidateAndThrow(categoriesCreateInput);
                    var categoryModel = mapper.Map<CategoryModel>(categoriesCreateInput);
                    return await categoryRepository.CreateAsync(categoryModel);
                });
            
            Field<CategoryType, CategoryModel>()
                .Name("Update")
                .Argument<NonNullGraphType<CategoriesUpdateInputType>, CategoriesUpdateInput>("CategoriesUpdateInputType", "Argument for Update Category")
                .ResolveAsync(async context => 
                {
                    var categoriesUpdateInput = context.GetArgument<CategoriesUpdateInput>("CategoriesUpdateInputType");
                    new CategoriesUpdateInputValidator().ValidateAndThrow(categoriesUpdateInput);
                    var categoryModel = mapper.Map<CategoryModel>(categoriesUpdateInput);
                    return await categoryRepository.UpdateAsync(categoryModel);
                });
            
            Field<CategoryType, CategoryModel>()
                .Name("Remove")
                .Argument<NonNullGraphType<IntGraphType>, int>("Id", "Argument for Remove Category")
                .ResolveAsync(async context => 
                {
                    int id = context.GetArgument<int>("Id");
                    return await categoryRepository.RemoveAsync(id);
                });
        }
    }
}
