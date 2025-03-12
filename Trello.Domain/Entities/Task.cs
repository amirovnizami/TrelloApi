using Trello.Domain.Abstractions;
using Trello.Domain.Enums;

namespace Trello.Domain.Entities;

public class Task:IEntity
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public Status Status { get; set; }

    public Priority Priority { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int CreatorId { get; set; }

    public int? AssigneeId { get; set; }

    public  User? Assignee { get; set; }

    public virtual User Creator { get; set; } = null!;
}
