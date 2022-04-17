using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public class CategoryModel : BaseModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public List<ToDoModel> ToDos { get; set; } = new List<ToDoModel>();


        [Display(Name = "User")]
        public int? UserId { get; set; }

        public UserModel? User { get; set; }
    }
}
