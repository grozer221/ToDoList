using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static ToDoList.ViewModels.ToDos.ToDosCreateViewModel;

namespace ToDoList.ViewModels.ToDos
{
    public class ToDosEditViewModel
    {
        public ToDoEditViewModel ToDo { get; set; }
        public List<CategoryModel> Categories { get; set; }
    }

    public class ToDoEditViewModel : ToDoCreateViewModel
    {
        [BindProperty(Name = "ToDo.Id")]
        [Required(ErrorMessage = "Id is required")]
        public int Id { get; set; }

        [BindProperty(Name = "ToDo.IsComplete")]
        [Display(Name = "Is done")]
        public bool IsComplete { get; set; }
    }
}
