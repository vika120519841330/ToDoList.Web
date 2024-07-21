using System.ComponentModel.DataAnnotations;
using ToDoList.Web.Data.Validation;

namespace ToDoList.Web.Data.DTO;

public class ToDoItemModel : ICloneable
{
    [Display(Name = "Идентификатор")]
    public int Id { get; set; }

    [Display(Name = "Наименование")]
    [StringLength(maximumLength: 50, MinimumLength = 5, ErrorMessage = "Описание не должно превышать 100 символов")]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "Описание")]
    [StringLength(200, ErrorMessage = "Описание не должно превышать 200 символов")]
    public string Description { get; set; }

    [Display(Name = "Выполнено")]
    public bool IsCompleted { get; set; }

    [Display(Name = "Срок")]
    public DateTime? DueDate { get; set; }

    [Display(Name = "Приоритет")]
    [ValidateIntNotZero(PropName = "Приоритет")]
    public int PriorityId { get; set; }

    [Display(Name = "Пользователь")]
    [ValidateIntNotZero(PropName = "Пользователь")]
    public int UserId { get; set; }

    public object Clone()
    => new ToDoItemFullModel
    {
        Id = this.Id,
        Title = this.Title,
        Description = this.Description,
        IsCompleted = this.IsCompleted,
        DueDate = this.DueDate,
        PriorityId = this.PriorityId,
        UserId = this.UserId,
    };
}
