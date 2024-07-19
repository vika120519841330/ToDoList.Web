using AutoMapper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ToDoList.Server.Data.Models.DBModels;

namespace ToDoList.Server.Data.Models.DTO.Response;

[AutoMap(typeof(User), ReverseMap = true)]
public class UserResponse
{
    public int Id { get; set; }

    public string Name { get; set; }
}
