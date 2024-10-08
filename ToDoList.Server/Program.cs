using Microsoft.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using ToDoList.Server.Configs;
using ToDoList.Server.Data.Context;
using ToDoList.Server.Data.Models.DBModels;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();
builder.Host.ConfigureSeqSerilog(builder.Environment);
builder.Environment.ConfigureSeqSerilog();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDBContext(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddServices();

builder.Services.AddRequestDecompression();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

PrimaryDataInit(app);

app.Run();

static void PrimaryDataInit(WebApplication app)
{
    using var context = app.Services.GetService<AppDbContext>();
    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();

    // Users
    var user1 = new User { Name = "User1" };
    var user2 = new User { Name = "User2" };
    var user3 = new User { Name = "User3" };
    context.Users.AddRange(user1, user2, user3);
    context.SaveChanges();

    // Priorities
    var pripity1 = new Priority { Level = 0 };
    var pripity2 = new Priority { Level = 1 };
    var pripity3 = new Priority { Level = 2 };
    var pripity4 = new Priority { Level = 3 };
    var pripity5 = new Priority { Level = 4 };
    context.Priorities.AddRange(pripity1, pripity2, pripity3, pripity4, pripity5);
    context.SaveChanges();

    // 
    var toDoItem1 = new ToDoItem
    {
        Title = "Task1",
        Description = "Description1",
        IsCompleted = false,
        DueDate = DateTime.Now.AddDays(25),
        PriorityId = pripity1.Id,
        Priority = pripity1,
        UserId = user1.Id,
        User = user1,
    };
    var toDoItem2 = new ToDoItem
    {
        Title = "Task2",
        Description = "Description2",
        IsCompleted = true,
        DueDate = DateTime.Now.AddDays(15),
        PriorityId = pripity3.Id,
        Priority = pripity3,
        UserId = user2.Id,
        User = user2,
    };
    context.ToDoItems.AddRange(toDoItem1, toDoItem2);
    context.SaveChanges();
}

