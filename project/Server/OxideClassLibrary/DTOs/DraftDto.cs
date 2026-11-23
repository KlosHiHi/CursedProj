namespace OxideClassLibrary.DTOs
{
    public class DraftDto
    {
        public Guid DraftId { get; set; }
        public string Title { get; set; } = null!;
        public Guid CategoryId { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastChangeTime { get; set; }
    }
}
