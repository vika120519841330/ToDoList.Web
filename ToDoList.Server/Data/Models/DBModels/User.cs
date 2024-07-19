using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoList.Server.Data.Models.DBModels;

[Table("users", Schema = "dbo")]
public class User
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; }

    public List<ToDoItem> ToDoItems { get; set; }
}
