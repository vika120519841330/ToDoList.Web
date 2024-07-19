using System.ComponentModel.DataAnnotations;

namespace ToDoList.Web.Data.DTO;

public class ToDoItemFullModel : ToDoItemModel
{
    [Display(Name = "Приоритет")]
    public int PriorityValue { get; set; }

    [Display(Name = "Пользователь")]
    public string UserName { get; set; } = string.Empty;
}
