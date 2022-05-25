using GraphQL.Types;
using ToDoList.GraphQL.Modules.Categories;

namespace ToDoList.GraphQL.Modules.ToDos
{
    public class ToDoType : ObjectGraphType<ToDoModel>
    {
        private readonly ICategoryRepository categoriesRepository;

        public ToDoType(IHttpContextAccessor httpContextAccessor, IEnumerable<ICategoryRepository> categoryRepositories)
        {
            string? dataProvider = httpContextAccessor.HttpContext.Request.Cookies["DataProvider"];
            categoriesRepository = categoryRepositories.GetPropered(dataProvider);

            Field<NonNullGraphType<IntGraphType>, int>()
               .Name("Id")
               .Resolve(context => context.Source.Id);

            Field<NonNullGraphType<StringGraphType>, string>()
               .Name("Name")
               .Resolve(context => context.Source.Name);

            Field<NonNullGraphType<BooleanGraphType>, bool>()
               .Name("IsComplete")
               .Resolve(context => context.Source.IsComplete);

            Field<DateTimeGraphType, DateTime?>()
               .Name("Deadline")
               .Resolve(context => context.Source.Deadline);

            Field<DateTimeGraphType, DateTime?>()
               .Name("DateComplete")
               .Resolve(context => context.Source.DateComplete);

            Field<IntGraphType, int?>()
               .Name("CategoryId")
               .Resolve(context => context.Source.CategoryId);

            Field<CategoryType, CategoryModel>()
               .Name("Category")
               .ResolveAsync(async context =>
               {
                   int? categoryId = context.Source.CategoryId;
                   if (categoryId == null)
                       return null;
                   return await categoriesRepository.GetByIdAsync((int)categoryId);
               });

            Field<NonNullGraphType<DateTimeGraphType>, DateTime>()
               .Name("CreatedAt")
               .Resolve(context => context.Source.CreatedAt);

            Field<NonNullGraphType<DateTimeGraphType>, DateTime>()
               .Name("UpdatedAt")
               .Resolve(context => context.Source.UpdatedAt);
        }
    }
}
