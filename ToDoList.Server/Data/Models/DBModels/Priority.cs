using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoList.Server.Data.Models.DBModels;

[Table("priorities", Schema = "dbo")]
public class Priority
{
    [Key]
    public int Id { get; set; }

    public int Level { get; set; }

    public List<ToDoItem> ToDoItems { get; set; }
}