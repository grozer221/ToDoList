using System.ComponentModel.DataAnnotations;

namespace ToDoList.ViewModels.Categories
{
    public class CategoriesEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
    }
}
