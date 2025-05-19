namespace Article_Review_System_backend.Models.Auth
{
    public class AuthResponse
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public string Gender { get; set; }

        public string? Specialization { get; set; }
        public string? Location { get; set; }
        public string? Bio { get; set; }
        public string? Twitter { get; set; }
        public string? LinkedIn { get; set; }
        public string? AvatarUrl { get; set; }
    }
}
