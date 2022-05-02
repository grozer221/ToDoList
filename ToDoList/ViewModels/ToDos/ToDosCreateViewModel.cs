using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.ViewModels.ToDos;

public class ToDosCreateViewModel 
{
    public ToDoCreateViewModel ToDo { get; set; }
    public List<CategoryModel> Categories { get; set; }


    public class ToDoCreateViewModel
    {
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
