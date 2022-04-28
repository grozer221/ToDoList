using System.ComponentModel.DataAnnotations;

namespace ToDoList.ViewModels.ToDos
{
    public class ToDosCreateViewModel 
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public DateTime? Deadline { get; set; }

        [Display(Name = "Category")]
        public int? CategoryId { get; set; }

        public List<CategoryModel>? Categories { get; set; }
    }
}
