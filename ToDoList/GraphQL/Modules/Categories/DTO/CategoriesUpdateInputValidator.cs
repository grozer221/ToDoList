using FluentValidation;

namespace ToDoList.GraphQL.Modules.Categories.DTO
{
    public class CategoriesUpdateInputValidator : AbstractValidator<CategoriesUpdateInput>
    {
        public CategoriesUpdateInputValidator()
        {
            RuleFor(c => c.Id)
                .NotNull();
            
            RuleFor(c => c.Name)
                .NotNull()
                .NotEmpty();
        }
    }
}
