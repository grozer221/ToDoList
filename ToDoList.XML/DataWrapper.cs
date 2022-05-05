using ToDoList.Business.Models;

namespace ToDoList.XML
{
    public class DataWrapper
    {
        public List<ToDoModel> ToDos { get; set; }
        public List<CategoryModel> Categories { get; set; }
    }
}
