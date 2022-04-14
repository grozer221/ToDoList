namespace ToDoList.Abstraction
{
    public class GetEntitiesResponse<T> where T : BaseModel
    {
        public List<T> Entities { get; set; }
        public int Total { get; set; }
        public int PageSize { get; set; }

    }
}
