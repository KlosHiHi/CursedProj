using System;
using System.Collections.Generic;

namespace OxideClassLibrary.Models;

public partial class User
{
    public Guid UserId { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime CreateTime { get; set; }

    public DateTime LastActiveTime { get; set; }

    public string? Email { get; set; }

    public bool IsVerify { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<UserNote> UserNotes { get; set; } = new List<UserNote>();

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

    public virtual ICollection<Draft> Drafts { get; set; } = new List<Draft>();
}
