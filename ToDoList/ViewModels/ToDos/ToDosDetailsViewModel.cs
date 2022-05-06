using System.ComponentModel.DataAnnotations;

namespace ToDoList.ViewModels.ToDos
{
    public class ToDosDetailsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsComplete { get; set; }

        public DateTime? Deadline { get; set; }

        public DateTime? DateComplete { get; set; }

        public int? CategoryId { get; set; }

        public CategoryModel? Category { get; set; }

        [Display(Name = "Created at")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Updated at")]
        public DateTime UpdatedAt { get; set; }
    }
}
