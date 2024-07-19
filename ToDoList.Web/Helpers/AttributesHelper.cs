using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ToDoList.Web.Helpers;

public static class AttributesHelper
{
    public static string GetDisplayValue(this Type modelType, string propName)
    {
        if (modelType == null) return default;

        var myprop = modelType.GetProperty(propName) as MemberInfo;

        if (myprop == null) return default;

        var attr = myprop.GetCustomAttribute(typeof(DisplayAttribute));

        if (attr == null) return default;

        return attr is DisplayAttribute ? (attr as DisplayAttribute).Name : string.Empty;
    }
}
