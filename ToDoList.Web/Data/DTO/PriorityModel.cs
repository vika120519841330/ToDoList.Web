using System.ComponentModel.DataAnnotations;

namespace ToDoList.Web.Data.DTO;

public class PriorityModel
{
    public int Id { get; set; }

    public int Level { get; set; }
}
