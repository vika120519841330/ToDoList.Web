using Microsoft.AspNetCore.Components;
using System.Reflection;

namespace ToDoList.Web.Components.Pages.Base;

public abstract class TSearchBase<T> : ComponentBase
{
    [Parameter]
    public EventCallback<List<T>> SendSelectedItems { get; set; }

    [Parameter]
    public List<T> InitParameters { get; set; }

    [Parameter]
    public string LabelForSearch { get; set; }

    [Parameter]
    public bool IsRenderLabelForSearch { get; set; }

    [Parameter]
    public PropertyInfo KeyPropInfo { get; set; }

    protected object GetKeyPropertyValue(T item) => item != null ? KeyPropInfo?.GetValue(item) ?? string.Empty : string.Empty;

    protected string GetStringKeyPropertyValue(T item) => GetKeyPropertyValue(item)?.ToString() ?? string.Empty;

    [Parameter]
    public PropertyInfo ValuePropInfo { get; set; }

    protected Type ValuePropInfoType => ValuePropInfo.PropertyType;

    protected object GetValuePropertyValue(T item) => item != null ? ValuePropInfo?.GetValue(item) ?? string.Empty : string.Empty;

    protected string GetStringValueProperty(T item) => GetValuePropertyValue(item)?.ToString() ?? string.Empty;

    protected bool ValuePropIsNullable
    => (KeyPropInfo?.GetGetMethod()?.ReturnType?.IsCollectible ?? false)
    || (KeyPropInfo?.GetGetMethod()?.ReturnType?.IsArray ?? false)
    || (KeyPropInfo?.GetGetMethod()?.ReturnType?.IsClass ?? false)
    || System.Nullable.GetUnderlyingType(KeyPropInfo?.GetGetMethod()?.ReturnType ?? default) != null
    || KeyPropInfo?.GetGetMethod()?.ReturnType == typeof(System.String);

    public List<T> SearchedItems { get; set; } = new();

    public string searchString = string.Empty;
    public string SearchString
    {
        get => this.searchString;
        set
        {
            this.searchString = value;
            Func<Task> f2 = async () => await this.SearchStart();
            f2();
        }
    }

    public abstract Task SearchStart();
}
