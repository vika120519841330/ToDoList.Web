using System.ComponentModel.DataAnnotations;

namespace ToDoList.Web.Data.DTO;

public class ToDoItemFullModel : ToDoItemModel, ICloneable
{
    [Display(Name = "Приоритет")]
    public int PriorityValue { get; set; }

    [Display(Name = "Пользователь")]
    public string UserName { get; set; } = string.Empty;

    public object Clone()
        => new ToDoItemFullModel
        {
            Id = this.Id,
            Title = this.Title,
            Description = this.Description,
            IsCompleted = this.IsCompleted,
            DueDate = this.DueDate,
            PriorityId = this.PriorityId,
            PriorityValue = this.PriorityValue,
            UserId = this.UserId,
            UserName = this.UserName,
        };
}
