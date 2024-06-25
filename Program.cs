// using Microsoft.AspNetCore.Builder;
// using Microsoft.AspNetCore.Http;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.EntityFrameworkCore;


// var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddDbContext<ToDoDbContext>(options =>
// {
//     options.UseMySql(builder.Configuration.GetConnectionString("ToDoDB"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("ToDoDB")));
// });

// var app = builder.Build();
// app.MapGet("/tasks", (ToDoDbContext dbContext) =>
// {
//     var tasks = dbContext.Tasks.ToList();
//     return tasks;
// });

// app.MapPost("/tasks", (ToDoDbContext dbContext, Task task) =>
// {
//     dbContext.Tasks.Add(task);
//     dbContext.SaveChanges();
//     return task;
// });

// app.MapPut("/tasks/{id}", (ToDoDbContext dbContext, int id, Task task) =>
// {
//     var existingTask = dbContext.Tasks.Find(id);
//     if (existingTask == null)
//     {
//         return Results.NotFound();
//     }

//     existingTask.Name = task.Name;
//     dbContext.SaveChanges();
//     return existingTask;
// });

// app.MapDelete("/tasks/{id}", (ToDoDbContext dbContext, int id) =>
// {
//     var task = dbContext.Tasks.Find(id);
//     if (task == null)
//     {
//         return Results.NotFound();
//     }

//     dbContext.Tasks.Remove(task);
//     dbContext.SaveChanges();
//     return Results.NoContent();
// });

// app.MapGet("/items", (ToDoDbContext dbContext) =>
// {
//     var items = dbContext.Items.ToList();
//     return items;
// });

// app.MapPost("/items", (ToDoDbContext dbContext, Item item) =>
// {
//     dbContext.Items.Add(item);
//     dbContext.SaveChanges();
//     return item;
// });

// app.MapPut("/items/{id}", (ToDoDbContext dbContext, int id, Item item) =>
// {
//     var existingItem = dbContext.Items.Find(id);
//     if (existingItem == null)
//     {
//         return Results.NotFound();
//     }

//     existingItem.Name = item.Name;
//     existingItem.IsComplete = item.IsComplete;
//     dbContext.SaveChanges();
//     return existingItem;
// });

// app.MapDelete("/items/{id}", (ToDoDbContext dbContext, int id) =>
// {
//     var item = dbContext.Items.Find(id);
//     if (item == null)
//     {
//         return
//             Results.NotFound();
//     }

//     dbContext.Items.Remove(item);
//     dbContext.SaveChanges();
//     return Results.NoContent();
// });

// public class ToDoDbContext : DbContext
// {
//     public ToDoDbContext(DbContextOptions<ToDoDbContext> options) : base(options) { }
//     public DbSet<Task> Tasks { get; set; }
//     public DbSet<Item> Items { get; set; }
// }

// var connectionString = builder.Configuration.GetConnectionString("ToDoDB");
// var serverVersion = ServerVersion.AutoDetect(connectionString);
// builder.Services.AddDbContext<ToDoDbContext>(options =>
// {
//     options.UseMySql(connectionString, serverVersion);
// });

// app.Run();

// using (var scope = app.Services.CreateScope())
// {
//     var dbContext = scope.ServiceProvider.GetRequiredService<ToDoDbContext>();
//     dbContext.Database.Migrate();
// }


using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;

public class ToDoDbContext : DbContext
{
    public ToDoDbContext(DbContextOptions<ToDoDbContext> options) : base(options) { }
    public DbSet<Task> Tasks { get; set; }
    public DbSet<Item> Items { get; set; }
}

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("ToDoDB");
var serverVersion = ServerVersion.AutoDetect(connectionString);
builder.Services.AddDbContext<ToDoDbContext>(options =>
{
    options.UseMySql(connectionString, serverVersion);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API Name", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API Name v1");
    });
}

// הוספת הרשאות CORS שמאפשרות גישה מכל מוצא
app.UseCors(builder =>
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader());

// המשך קוד ה-API כאן

app.MapGet("/tasks", (ToDoDbContext dbContext) =>
{
    var tasks = dbContext.Tasks.ToList();
    return tasks;
});

app.MapPost("/tasks", (ToDoDbContext dbContext, Task task) =>
{
    dbContext.Tasks.Add(task);
    dbContext.SaveChanges();
    return task;
});

app.MapPut("/tasks/{id}", (ToDoDbContext dbContext, int id, Task task) =>
{
    var existingTask = dbContext.Tasks.Find(id);
    if (existingTask == null)
    {
        return Results.NotFound();
    }

    existingTask.Name = task.Name;
    dbContext.SaveChanges();
    return existingTask;
});

app.MapDelete("/tasks/{id}", (ToDoDbContext dbContext, int id) =>
{
    var task = dbContext.Tasks.Find(id);
    if (task == null)
    {
        return Results.NotFound();
    }

    dbContext.Tasks.Remove(task);
    dbContext.SaveChanges();
    return Results.NoContent();
});

app.MapGet("/items", (ToDoDbContext dbContext) =>
{
    var items = dbContext.Items.ToList();
    return items;
});

app.MapPost("/items", (ToDoDbContext dbContext, Item item) =>
{
    dbContext.Items.Add(item);
    dbContext.SaveChanges();
    return item;
});

app.MapPut("/items/{id}", (ToDoDbContext dbContext, int id, Item item) =>
{
    var existingItem = dbContext.Items.Find(id);
    if (existingItem == null)
    {
        return Results.NotFound();
    }

    existingItem.Name = item.Name;
    existingItem.IsComplete = item.IsComplete;
    dbContext.SaveChanges();
    return existingItem;
});

app.MapDelete("/items/{id}", (ToDoDbContext dbContext, int id) =>
{
    var item = dbContext.Items.Find(id);
    if (item == null)
    {
        return Results.NotFound();
    }

    dbContext.Items.Remove(item);
    dbContext.SaveChanges();
    return Results.NoContent();
});

// הפעלת המיגרציה
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ToDoDbContext>();
    dbContext.Database.Migrate();
}

app.Run();
