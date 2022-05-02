namespace ToDoList.Business.Models;

public class CategoryModel : BaseModel
{
    public string Name { get; set; }
    public int? UserId { get; set; }
    public UserModel? User { get; set; }
    public List<ToDoModel> ToDos { get; set; } = new List<ToDoModel>();
}
