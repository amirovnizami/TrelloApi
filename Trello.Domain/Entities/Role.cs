using System;
using System.Collections.Generic;
using Trello.Domain.Entities;

namespace Trello.DAL.SqlServer.Models;

public partial class Role
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
