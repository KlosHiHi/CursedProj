namespace OxideClassLibrary.DTOs
{
    public class NoteDto
    {
        public Guid NoteId { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string Role { get; set; } = null!;
        public Guid CategoryId { get; set; }
        public bool IsCommented { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastChangeTime { get; set; }
    }
}
