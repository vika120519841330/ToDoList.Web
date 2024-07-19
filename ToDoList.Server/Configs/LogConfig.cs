using Serilog.Sinks.SystemConsole.Themes;
using Serilog;
using Microsoft.AspNetCore;
using Serilog.Exceptions;

namespace ToDoList.Server.Configs;

public static class LogConfig
{
    private const string hostDevelop = "http://localhost:5341";
    private const string hostAnother = "https://xxx.xx";
    public static void ConfigureSeqSerilog(this IHostBuilder builder, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            builder.UseSerilog((ctx, lc) => lc
                .WriteTo.Console()
                .WriteTo.Seq(hostDevelop, Serilog.Events.LogEventLevel.Debug));
        }
        else
        {
            builder.UseSerilog((ctx, lc) => lc
                .WriteTo.Console()
                .WriteTo.Seq(hostAnother, Serilog.Events.LogEventLevel.Warning));
        }
    }

    public static void ConfigureSeqSerilog(this IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.WithExceptionDetails()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProcessId()
                .Enrich.WithProperty("Application", env.ApplicationName)
                .Enrich.WithProperty("Environment", env.EnvironmentName)
                .WriteTo.Seq(hostDevelop)
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{NotifyMessage:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Code).CreateLogger();
        }
        else
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Warning()
                .Enrich.WithExceptionDetails()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProcessId()
                .Enrich.WithProperty("Application", env.ApplicationName)
                .Enrich.WithProperty("Environment", env.EnvironmentName)
                .WriteTo.Seq(hostAnother)
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{NotifyMessage:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Code).CreateLogger();
        }
    }
}
