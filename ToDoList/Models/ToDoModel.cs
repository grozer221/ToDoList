using ToDoList.ViewModels.ToDos;

namespace ToDoList.Models
{
    public class ToDoModel : BaseModel
    {
        public ToDoModel()
        {
        }
        
        public ToDoModel(ToDosCreateViewModel toDosCreateViewModel)
        {
            Name = toDosCreateViewModel.Name;
            Deadline = toDosCreateViewModel.Deadline;
            CategoryId = toDosCreateViewModel.CategoryId;
        }

        public ToDoModel(ToDosEditViewModel toDosEditViewModel)
        {
            Id = toDosEditViewModel.Id;
            Name = toDosEditViewModel.Name;
            IsComplete = toDosEditViewModel.IsComplete;
            Deadline = toDosEditViewModel.Deadline;
            CategoryId = toDosEditViewModel.CategoryId;
        }

        public string Name { get; set; }
        public bool IsComplete { get; set; }
        public DateTime? Deadline { get; set; }
        public DateTime? DateComplete { get; set; }
        public int? CategoryId { get; set; }
        public CategoryModel? Category { get; set; }
        public int? UserId { get; set; }
        public UserModel? User { get; set; }
    }
}
