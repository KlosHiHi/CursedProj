using System;
using System.Collections.Generic;

namespace OxideClassLibrary.Models;

public partial class Role
{
    public int RoleId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<UserNote> UserNotes { get; set; } = new List<UserNote>();
}
