using FluentValidation;

namespace ToDoList.GraphQL.Modules.Categories.DTO
{
    public class CategoriesCreateInputValidator : AbstractValidator<CategoriesCreateInput>
    {
        public CategoriesCreateInputValidator()
        {
            RuleFor(c => c.Name)
                .NotNull()
                .NotEmpty();
        }
    }
}
