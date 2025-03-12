using System.ComponentModel.DataAnnotations;
using Trello.DAL.SqlServer.Models;
using Trello.Domain.Abstractions;
using Task = Trello.Domain.Entities.Task;

namespace Trello.Domain.Entities;

public  class User:IEntity
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public int RoleId { get; set; } 

    public Role Role { get; set; }
}
