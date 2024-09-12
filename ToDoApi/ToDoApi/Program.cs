using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ToDoApi.Db;
using ToDoApi.Enums;
using ToDoApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Register Dependency Injection - AddService Methods
builder.Services.AddDbContext<TodoDbContext>(options => options.UseInMemoryDatabase("Todos"));

// DI - Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options=> {
    options.SwaggerDoc("v1", new OpenApiInfo() { Title = "Todo Minimal Api", Version = "v1" });
});

var app = builder.Build();

// Configure request/response pipelines - Use and Map methods

// Build Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c=> c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo Minimal Api v1"));
}

app.MapGet("/", () => "Welcome to Minimal Apis in .Net8");


app.MapGet("/todos", async(TodoDbContext todoDbContext)=> await todoDbContext.Todos.ToListAsync());

app.MapGet("/todo{Id}", async (TodoDbContext todoDbContext, Guid Id) => 
await todoDbContext.Todos.FindAsync(Id));

app.MapPost("/todos", async (TodoDbContext todoDbContext, TodoItem todoItem) =>
{
    todoItem.CreatedDateUtc = DateTime.UtcNow;
    todoItem.Status = TodoItemStatus.Open;
    todoDbContext.Todos.Add(todoItem);
    await todoDbContext.SaveChangesAsync();
    return Results.Created($"/todo/{todoItem.Id}", todoItem);
});

app.MapPut("/todo{Id}", async (TodoDbContext todoDbContext, TodoItem todoItem, Guid Id) =>
{
    var todo = await todoDbContext.Todos.FindAsync(Id);
    if (todo == null) return Results.NotFound();
    todo.Description = todoItem.Description;
    todo.Title = todoItem.Title;
    todo.UpdatedDateUtc = DateTime.UtcNow;
    todo.Status = todoItem.Status;    
    await todoDbContext.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/todo{Id}", async (TodoDbContext todoDbContext, Guid Id) =>
{
    var todo = await todoDbContext.Todos.FindAsync(Id);
    if (todo == null) return Results.NotFound();
    todoDbContext.Todos.Remove(todo);
    await todoDbContext.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();
