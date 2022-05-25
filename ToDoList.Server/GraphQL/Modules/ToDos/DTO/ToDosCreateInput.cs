namespace ToDoList.GraphQL.Modules.ToDos.DTO
{
    public class ToDosCreateInput
    {
        public string Name { get; set; }
        public DateTime? Deadline { get; set; }
        public int? CategoryId { get; set; }
    }
}
