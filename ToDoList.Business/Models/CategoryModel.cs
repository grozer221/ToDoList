namespace ToDoList.Business.Models;

public class CategoryModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<ToDoModel> ToDos { get; set; } = new List<ToDoModel>();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
