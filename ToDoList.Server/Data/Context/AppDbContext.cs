using Microsoft.EntityFrameworkCore;
using ToDoList.Server.Data.Models.DBModels;

namespace ToDoList.Server.Data.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<ToDoItem> ToDoItems { get; set; }
    public DbSet<Priority> Priorities { get; set; }
    public DbSet<User> Users { get; set; }

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
}
