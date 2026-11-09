using System;
using System.Collections.Generic;

namespace OxideClassLibrary.Models;

public partial class UserNote
{
    public Guid UserId { get; set; }

    public Guid NoteId { get; set; }

    public int RoleId { get; set; }

    public virtual Note Note { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
