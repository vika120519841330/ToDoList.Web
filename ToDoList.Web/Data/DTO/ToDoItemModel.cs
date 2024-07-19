using System.ComponentModel.DataAnnotations;

namespace ToDoList.Web.Data.DTO;

public class ToDoItemModel
{
    [Display(Name = "Идентификатор")]
    public int Id { get; set; }

    [Display(Name = "Наименование")]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "Описание")]
    public string Description { get; set; }

    [Display(Name = "Выполнено")]
    public bool IsCompleted { get; set; }

    [Display(Name = "Срок")]
    public DateTime DueDate { get; set; }

    public int PriorityId { get; set; }

    public int UserId { get; set; }
}
