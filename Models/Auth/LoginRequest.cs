using System.ComponentModel.DataAnnotations;

namespace Article_Review_System_backend.Models.Auth
{
    public class LoginRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}