using System;
using System.Collections.Generic;

namespace OxideClassLibrary.Models;

public partial class Note
{
    public Guid NoteId { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public Guid CategoryId { get; set; }

    public bool IsCommented { get; set; }

    public DateTime CreationTime { get; set; }

    public DateTime LastChangeTime { get; set; }

    public bool IsArhived { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<UserNote> UserNotes { get; set; } = new List<UserNote>();
}
