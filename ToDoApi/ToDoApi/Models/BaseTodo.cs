namespace ToDoApi.Models
{
    public class BaseTodo
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime? CreatedDateUtc { get; set; }
        public DateTime? UpdatedDateUtc { get; set; }
    }
}
