namespace StudentAPI.Models
{
    public class LoginRequest
    {
        public string? Role { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
