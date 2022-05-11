namespace ToDoList.GraphQL.Modules.Categories.DTO
{
    public class CategoriesUpdateInput : CategoriesCreateInput
    {
        public int Id { get; set; }
    }
}
