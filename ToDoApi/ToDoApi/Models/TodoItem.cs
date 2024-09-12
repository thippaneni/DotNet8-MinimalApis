using ToDoApi.Enums;

namespace ToDoApi.Models;

public class TodoItem : BaseTodo
{
    public string? Title { get; set; }
    public string? Description { get; set; }    
    public TodoItemStatus Status { get; set; }
}

