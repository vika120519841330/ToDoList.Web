using System.ComponentModel.DataAnnotations;

namespace ToDoList.Web.Data.DTO;

public class ToDoItemModel
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool IsCompleted { get; set; }

    public DateTime DueDate { get; set; }

    public int PriorityId { get; set; }

    public int UserId { get; set; }
}
