using AutoMapper;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ToDoList.Server.Data.Models.DBModels;

namespace ToDoList.Server.Data.Models.DTO.Response;

[AutoMap(typeof(ToDoItem), ReverseMap = true)]
public class ToDoItemResponse
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; }

    public bool IsCompleted { get; set; }

    public DateTime? DueDate { get; set; }

    public int PriorityId { get; set; }

    public int PriorityValue { get; set; }

    public int UserId { get; set; }

    public string UserName { get; set; } = string.Empty;
}
