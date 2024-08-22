using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using ToDoList.RabbitMQ.Interfaces;

namespace ToDoList.RabbitMQ.MQObjects;

public class MQToDoItem : MQObject
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; }

    public bool IsCompleted { get; set; }

    public DateTime? DueDate { get; set; }

    public int PriorityId { get; set; }

    public int UserId { get; set; }

    public override string ToString()
        => JsonSerializer.SerializeToElement(this).ToString();
}
