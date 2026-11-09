using System;
using System.Collections.Generic;

namespace OxideClassLibrary.Models;

public partial class Draft
{
    public Guid DraftId { get; set; }

    public string Title { get; set; } = null!;

    public Guid CategoryId { get; set; }

    public DateTime LastChangeTime { get; set; }

    public DateTime CreationTime { get; set; }

    public bool IsArhived { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
