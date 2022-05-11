using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.ViewModels.ToDos
{
    public class ToDosEditViewModel
    {
        public ToDoEditViewModel ToDo { get; set; }
        public IEnumerable<CategoryModel> Categories { get; set; }
    }

    public class ToDoEditViewModel : ToDosCreateViewModel
    {
        [BindProperty(Name = "ToDo.Id")]
        [Required(ErrorMessage = "Id is required")]
        public int Id { get; set; }

        [BindProperty(Name = "ToDo.IsComplete")]
        [Display(Name = "Is complete")]
        public bool IsComplete { get; set; }

        [BindProperty(Name = "ToDo.Name")]
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [BindProperty(Name = "ToDo.Deadline")]
        public DateTime? Deadline { get; set; }

        [BindProperty(Name = "ToDo.CategoryId")]
        [Display(Name = "Category")]
        public int? CategoryId { get; set; }
    }
}
