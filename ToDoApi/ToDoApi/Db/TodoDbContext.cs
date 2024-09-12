namespace ToDoApi.Db;

using Microsoft.EntityFrameworkCore;
using ToDoApi.Models;

public class TodoDbContext : DbContext
{
    public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
    {        
    }

    public DbSet<TodoItem> Todos { get; set; }
}
