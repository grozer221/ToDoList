namespace ToDoList.GraphQL.Modules.ToDos.DTO
{
    public class ToDosUpdateInput
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? Deadline { get; set; }
        public int? CategoryId { get; set; }
        public bool IsComplete { get; set; }
    }
}
