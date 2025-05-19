using System.ComponentModel.DataAnnotations;

namespace Article_Review_System_backend.Models.Auth
{
    public class RegisterRequest
    {
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }
        
        [Required]
        public string Gender { get; set; }
        
        [Required, EmailAddress]
        public string Email { get; set; }
        
        [Required, MinLength(8)]
        public string Password { get; set; }

        public string? Role { get; set; }
    }
}