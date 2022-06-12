using ToDoList.Business.Abstractions;

namespace ToDoList.Business.Models;

public class ToDoModel: BaseModel
{
    public string Name { get; set; }
    public bool IsComplete { get; set; }
    public DateTime? Deadline { get; set; }
    public DateTime? DateComplete { get; set; }
    public int? CategoryId { get; set; }
    public CategoryModel? Category { get; set; }
}
