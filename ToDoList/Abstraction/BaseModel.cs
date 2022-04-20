using System.ComponentModel.DataAnnotations;

namespace ToDoList.Abstraction
{
    public abstract class BaseModel
    {
        public int Id { get; set; }

        [Display(Name = "Created at")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Updated at")]
        public DateTime UpdatedAt { get; set; }
    }
}
