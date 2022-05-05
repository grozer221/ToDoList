using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.ViewModels.ToDos;

public class ToDosCreateViewModel 
{
    [BindProperty(Name = "CreateToDo.Name")]
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }

    [BindProperty(Name = "CreateToDo.Deadline")]
    public DateTime? Deadline { get; set; }

    [BindProperty(Name = "CreateToDo.CategoryId")]
    [Display(Name = "Category")]
    public int? CategoryId { get; set; }
}
