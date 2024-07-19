using AutoMapper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ToDoList.Server.Data.Models.DBModels;

namespace ToDoList.Server.Data.Models.DTO.Response;

[AutoMap(typeof(Priority), ReverseMap = true)]
public class PriorityResponse
{
    public int Id { get; set; }

    public int Level { get; set; }
}