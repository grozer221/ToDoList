namespace ToDoList.GraphQL.Modules.ToDos.DTO
{
    public class ToDosUpdateInput : ToDosCreateInput
    {
        public int Id { get; set; }
        public bool IsComplete { get; set; }
    }
}
