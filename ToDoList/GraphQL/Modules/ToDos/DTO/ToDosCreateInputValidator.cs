using FluentValidation;

namespace ToDoList.GraphQL.Modules.ToDos.DTO
{
    public class ToDosCreateInputValidator : AbstractValidator<ToDosCreateInput>
    {
        public ToDosCreateInputValidator()
        {
            RuleFor(c => c.Name)
                .NotNull()
                .NotEmpty();

            RuleFor(c => c.Deadline);

            RuleFor(c => c.CategoryId);
        }
    }
}
