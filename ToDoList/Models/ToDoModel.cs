using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public class ToDoModel : BaseModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Display(Name = "Is done")]
        public bool IsDone { get; set; }

        public DateTime? Deadline { get; set; }

        [Display(Name = "Category")]
        public int? CategoryId { get; set; }

        public CategoryModel? Category { get; set; }
    }
}
