using System;
using System.Collections.Generic;

namespace OxideClassLibrary.Models;

public partial class Category
{
    public Guid CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public string IconName { get; set; } = null!;

    public virtual ICollection<Draft> Drafts { get; set; } = [];

    public virtual ICollection<Note> Notes { get; set; } = [];

    public virtual ICollection<User> Users { get; set; } = [];
}
