using Serilog;

namespace ToDoList.Server.Helpers;

public static class ExceptionHelper
{
    public static void LogError(this Exception exc, string sourceName, string methodName, string message = null)
    {
        Log.Error($"Исключительная ситуация при выполнении метода «{methodName}» " +
                  $"внутри «{sourceName ?? string.Empty}». " +
                  $"Тип: «{exc?.GetType()?.FullName ?? string.Empty}». " +
                  $"Подробности: {message ?? string.Empty} " +
                  $"{exc.GetExeceptionMessages()} ");
    }

    public static string GetExeceptionMessages(this Exception exc)
    {
        return $"{exc?.Message ?? string.Empty}; {exc?.InnerException?.Message ?? string.Empty} ";
    }
}