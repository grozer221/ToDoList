using System.ComponentModel.DataAnnotations;

namespace ToDoList.ViewModels.ToDos
{
    public class ToDosDeleteViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Display(Name = "Is complete")]
        public bool IsComplete { get; set; }

        public DateTime? Deadline { get; set; }

        [Display(Name = "Date complete")]
        public DateTime? DateComplete { get; set; }


        [Display(Name = "Created at")]
        public DateTime CreatedAt { get; set; }
        
        [Display(Name = "Updated at")]
        public DateTime UpdatedAt { get; set; }
    }
}
