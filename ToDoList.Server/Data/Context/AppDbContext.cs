using Microsoft.EntityFrameworkCore;

namespace ToDoList.Server.Data.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}
