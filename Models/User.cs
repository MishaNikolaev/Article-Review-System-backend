
using System.ComponentModel.DataAnnotations;

namespace Article_Review_System_backend.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; }

        [Required]
        public byte[] PasswordSalt { get; set; }

        public string? Specialization { get; set; }
        public string? Location { get; set; }
        public string? Bio { get; set; }
        public string? Twitter { get; set; }
        public string? LinkedIn { get; set; }
        public string? AvatarUrl { get; set; }
        public string Role { get; set; } = "Author";

        public bool IsBlocked { get; set; } = false;


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public List<int> Reviews { get; set; } = [];

        public List<int> ReviewsFinished { get; set; } = [];

    }
}