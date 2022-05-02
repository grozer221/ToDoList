using System.ComponentModel.DataAnnotations;
using static ToDoList.ViewModels.ToDos.ToDosIndexViewModel;

namespace ToDoList.ViewModels.Categories
{
    public class CategoriesDetailsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public List<ToDoIndexViewModel> ToDos { get; set; }

        [Display(Name = "Created at")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Updated at")]
        public DateTime UpdatedAt { get; set; }
    }
}
