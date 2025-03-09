using Trello.DAL.SqlServer.Models;
using Trello.Domain.Abstractions;
using Task = Trello.DAL.SqlServer.Models.Task;

namespace Trello.Domain.Entities;

public partial class User:IEntity
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public int RoleId { get; set; }
    public virtual Role Role { get; set; } = null!;
}
