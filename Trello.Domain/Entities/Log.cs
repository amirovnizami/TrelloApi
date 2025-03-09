using System;
using System.Collections.Generic;
using Trello.Domain.Entities;

namespace Trello.DAL.SqlServer.Models;

public partial class Log
{
    public int Id { get; set; }

    public string Action { get; set; } = null!;

    public int UserId { get; set; }

    public DateTime? Timestamp { get; set; }

    public virtual User User { get; set; } = null!;
}
