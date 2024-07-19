using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoList.Server.Data.Models.DBModels;

[Table("to_do_items", Schema = "dbo")]
public class ToDoItem
{
    [Key]
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; }

    [Column("is_completed")]
    public bool IsCompleted { get; set; }

    [Column("due_date")]
    public DateTime DueDate { get; set; }

    [Column("priority_id")]
    public int PriorityId { get; set; }

    public Priority Priority { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    public User User { get; set; }
}
