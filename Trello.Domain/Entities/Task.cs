using System;
using System.Collections.Generic;
using Trello.Domain.Entities;

namespace Trello.DAL.SqlServer.Models;

public partial class Task
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int Status { get; set; }

    public int Priority { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int CreatorId { get; set; }

    public int? AssigneeId { get; set; }

    public virtual User? Assignee { get; set; }

    public virtual User Creator { get; set; } = null!;
}
