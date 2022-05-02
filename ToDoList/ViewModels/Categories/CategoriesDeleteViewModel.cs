using System.ComponentModel.DataAnnotations;

namespace ToDoList.ViewModels.Categories
{
    public class CategoriesDeleteViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Display(Name = "Created at")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Updated at")]
        public DateTime UpdatedAt { get; set; }
    }
}
