using Microsoft.EntityFrameworkCore;
using ToDoList.Server.Data.Models.DBModels;

namespace ToDoList.Server.Data.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();

        PrimaryDataInit();
    }

    DbSet<ToDoItem> ToDoItems { get; set; }
    DbSet<Priority> Priorities { get; set; }
    DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ToDoItem>()
            .HasOne(p => p.User)
            .WithMany(t => t.ToDoItems)
            .HasForeignKey(p => p.UserId);

        modelBuilder.Entity<ToDoItem>()
            .HasOne(p => p.Priority)
            .WithMany(t => t.ToDoItems)
            .HasForeignKey(p => p.PriorityId);

        modelBuilder.Entity<Priority>()
            .HasIndex(p => p.Level)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(p => p.Name)
            .IsUnique();
    }

    private void PrimaryDataInit()
    {
        // Users
        var user1 = new User { Name = "User1" };
        var user2 = new User { Name = "User2" };
        var user3 = new User { Name = "User3" };
        Users.AddRange(user1, user2, user3);
        SaveChanges();

        // Priorities
        var pripity1 = new Priority { Level = 0 };
        var pripity2 = new Priority { Level = 1 };
        var pripity3 = new Priority { Level = 2 };
        var pripity4 = new Priority { Level = 3 };
        var pripity5 = new Priority { Level = 4 };
        Priorities.AddRange(pripity1, pripity2, pripity3, pripity4, pripity5);
        SaveChanges();

        // 
        var toDoItem1 = new ToDoItem 
        {
            Title = "Task1", 
            Description = "Description1",
            IsCompleted = false,
            DueDate = DateTime.Now.AddDays(25),
            PriorityId = pripity1.Id,
            Priority = pripity1,
            UserId =  user1.Id,
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
        ToDoItems.AddRange(toDoItem1, toDoItem2);
        SaveChanges();
    }
}
