namespace OxideClassLibrary.Models;

public partial class UserNote
{
    public Guid UserId { get; set; }
    public Guid NoteId { get; set; }

    public string Role { get; set; } = null!;

    public virtual Note Note { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}
