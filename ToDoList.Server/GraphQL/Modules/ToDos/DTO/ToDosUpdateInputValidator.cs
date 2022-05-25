using FluentValidation;

namespace ToDoList.GraphQL.Modules.ToDos.DTO
{
    public class ToDosUpdateInputValidator : AbstractValidator<ToDosUpdateInput>
    {
        public ToDosUpdateInputValidator()
        {
            RuleFor(c => c.Id)
                .NotNull();
            
            RuleFor(c => c.IsComplete)
                .NotNull();
            
            RuleFor(c => c.Name)
                .NotNull()
                .NotEmpty();

            RuleFor(c => c.Deadline);

            RuleFor(c => c.CategoryId);
        }
    }
}
