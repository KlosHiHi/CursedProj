namespace OxideClassLibrary.DTOs
{
    public class RegistrationRequest
    {
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Email { get; set; }
    }
}
