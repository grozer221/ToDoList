using System.ComponentModel.DataAnnotations;

namespace ToDoList.ViewModels.Categories
{
    public class CategoriesCreateViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
    }
}
