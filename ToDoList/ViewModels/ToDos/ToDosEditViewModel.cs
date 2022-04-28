using System.ComponentModel.DataAnnotations;

namespace ToDoList.ViewModels.ToDos
{
    public class ToDosEditViewModel
    {
        public ToDosEditViewModel()
        {
        }
        
        public ToDosEditViewModel(ToDoModel toDo)
        {
            Id = toDo.Id;
            Name = toDo.Name;
            IsComplete = toDo.IsComplete;
            Deadline = toDo.Deadline;
            CategoryId = toDo.CategoryId;
        }

        [Required(ErrorMessage = "Id is required")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Display(Name = "Is done")]
        public bool IsComplete { get; set; }

        public DateTime? Deadline { get; set; }

        [Display(Name = "Category")]
        public int? CategoryId { get; set; }

        public List<CategoryModel>? Categories { get; set; }
    }
}
