using System.ComponentModel.DataAnnotations;

namespace ToDoList.ViewModels.ToDos
{
    public class ToDosIndexViewModel 
    {
        public ToDosCreateViewModel CreateToDo { get; set; }
        public IEnumerable<ToDoIndexViewModel> ToDos { get; set; }
        public IEnumerable<CategoryModel> Categories { get; set; }

        public class ToDoIndexViewModel
        {
            public int Id { get; set; }

            public string Name { get; set; }

            [Display(Name = "Is complete")]
            public bool IsComplete { get; set; }

            public DateTime? Deadline { get; set; }

            [Display(Name = "Date complete")]
            public DateTime? DateComplete { get; set; }

            [Display(Name = "Category")]
            public int? CategoryId { get; set; }

            public CategoryModel? Category { get; set; }

            [Display(Name = "Created at")]
            public DateTime CreatedAt { get; set; }
        }
    }
}
