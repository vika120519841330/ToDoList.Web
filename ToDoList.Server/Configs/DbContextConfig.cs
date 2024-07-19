using Microsoft.EntityFrameworkCore;
using System;
using ToDoList.Server.Data.Context;

namespace ToDoList.Server.Configs;

public static class DbContextConfig
{
    public static void AddDBContext(this IServiceCollection services, IConfiguration configuration)
    {
        var optionBuilder = (DbContextOptionsBuilder options) =>
        {
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
            options.UseSqlServer(
                configuration.GetConnectionString("AppDbContext"),
                builder =>
                {
                    builder.CommandTimeout(120);
                    builder.EnableRetryOnFailure();
                });
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        };

        services.AddDbContext<AppDbContext>(
            options => optionBuilder(options), ServiceLifetime.Transient);

        services.AddDbContextFactory<AppDbContext>(
            options => optionBuilder(options), ServiceLifetime.Transient);
    }

}
