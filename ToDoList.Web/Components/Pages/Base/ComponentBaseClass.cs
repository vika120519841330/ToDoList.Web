using Microsoft.AspNetCore.Components;
using ToDoList.Web.Data.DTO;

namespace ToDoList.Web.Components.Pages.Base;

public class ComponentBaseClass<T> : ComponentBase, IDisposable
{
    protected Type ModelType { get; } = typeof(T);

    private CancellationTokenSource TokenSource => new();

    protected CancellationToken Token => TokenSource?.Token ?? default;

    public bool IsRender { get; set; }

    public string Message { get; set; }

    public string Title { get; set; }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        // Отправка потребителям токена запроса на отмену.
        TokenSource?.Cancel();
        TokenSource?.Dispose();
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }
}
