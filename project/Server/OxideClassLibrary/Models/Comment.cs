using System;
using System.Collections.Generic;

namespace OxideClassLibrary.Models;

public partial class Comment
{
    public Guid CommentId { get; set; }

    public Guid UserId { get; set; }

    public Guid NoteId { get; set; }

    public string Content { get; set; } = null!;

    public virtual Note Note { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
