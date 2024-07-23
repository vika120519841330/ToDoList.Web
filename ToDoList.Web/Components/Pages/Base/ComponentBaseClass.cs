using AutoMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Caching.Distributed;
using ToDoList.Web.Data.DTO;

namespace ToDoList.Web.Components.Pages.Base;

public class ComponentBaseClass : ComponentBase, IDisposable
{
    [Inject]
    public IMapper Mapper { get; set; }

    private CancellationTokenSource TokenSource => new();

    protected CancellationToken Token => TokenSource?.Token ?? default;

    public bool IsRender { get; set; }

    public string Message { get; set; }

    public string Title { get; set; }

    protected bool ShowMessage { get; set; }

    protected bool ShowEditForm { get; set; }

    protected bool ShowDeleteConfarmation { get; set; }

    protected bool ShouldUpdate { get; set; }

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

    protected async Task RenderMessage()
    {
        ShowMessage = true;
        await InvokeAsync(StateHasChanged);
    }
}
