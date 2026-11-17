namespace OxideClassLibrary.DTOs
{
    public class UserData
    {
        public Guid UserId { get; set; }
        public string Login { get; set; } = null!;
        public DateTime CreateTime { get; set; }
        public DateTime LastActiveTime { get; set; }
        public string? Email { get; set; }
        public bool IsVerify { get; set; }
    }
}
