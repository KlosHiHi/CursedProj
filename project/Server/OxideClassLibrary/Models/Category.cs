using System;
using System.Collections.Generic;

namespace OxideClassLibrary.Models;

public partial class Category
{
    public Guid CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public string IconName { get; set; } = null!;

    public virtual ICollection<Draft> Drafts { get; set; } = new List<Draft>();

    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
